import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import monitor, {
  AlertData,
  DeviceOrientation,
  EnabledVitalSigns,
  FaceSessionOptions,
  HealthMonitorCodes,
  HealthMonitorSession,
  ImageValidity,
  OfflineMeasurements,
  SessionState,
  VitalSigns,
  VitalSignsResults,
} from '@binah/web-sdk';

export enum InfoType {
  NONE = 'NONE',
  INSTRUCTION = 'INSTRUCTION',
}

export interface InfoData {
  type: InfoType;
  message?: string;
}

@Injectable({
  providedIn: 'root',
})
export class MonitorService {
  private session: HealthMonitorSession | undefined;
  private sessionState = new BehaviorSubject<SessionState>(undefined!);
  private isMonitorReady = new BehaviorSubject<boolean>(false);
  private enabledVitalSigns = new BehaviorSubject<EnabledVitalSigns>(undefined!);
  private offlineMeasurements = new BehaviorSubject<OfflineMeasurements>(undefined!);
  private vitalSigns = new BehaviorSubject<VitalSigns | null>(null);
  private error = new BehaviorSubject<AlertData>({ code: -1 });
  private warning = new BehaviorSubject<AlertData>({ code: -1 });
  private info = new BehaviorSubject<InfoData>({ type: InfoType.NONE });
  private isDismissing = false;

  sessionState$ = this.sessionState.asObservable();
  isMonitorReady$ = this.isMonitorReady.asObservable();
  enabledVitalSigns$ = this.enabledVitalSigns.asObservable();
  offlineMeasurements$ = this.offlineMeasurements.asObservable();
  vitalSigns$ = this.vitalSigns.asObservable();
  error$ = this.error.asObservable();
  warning$ = this.warning.asObservable();
  info$ = this.info.asObservable();

  constructor() { }

  async initializeMonitor(licenseKey: string, productId: string) {
    try {
      await monitor.initialize({
        licenseKey,
        licenseInfo: {
          onEnabledVitalSigns: this.onEnabledVitalSigns.bind(this),
          onOfflineMeasurement: this.onOfflineMeasurement.bind(this),
        },
      });
      console.log('Initialized monitor');
      this.isMonitorReady.next(true);
      this.error.next({ code: -1 });
    } catch (e) {
      console.error('Error initializing HealthMonitor', e);
      this.isMonitorReady.next(false);
      const error = e as { errorCode: number };
      this.error.next({ code: error.errorCode });
    }
  }
  getProcessingTime(): Observable<number> {
    return of(60);
  }

  getLicenseKey(): Observable<string> {
    return of('C5A15F-F168F2-4633BE-BFF65F-E238FB-43CD12');
  }

  async createSession(video: HTMLVideoElement, cameraId: string, processingTime: number) {
    try {
      if (!this.isMonitorReady.value || !processingTime || !video) {
        return;
      }

      if (this.sessionState.value === SessionState.ACTIVE) {
        this.session!.terminate();
      }

      const options: FaceSessionOptions = {
        input: video,
        cameraDeviceId: cameraId,
        processingTime,
        onVitalSign: this.onVitalSign.bind(this),
        onFinalResults: this.onFinalResults.bind(this),
        onError: this.onError.bind(this),
        onWarning: this.onWarning.bind(this),
        onStateChange: this.onStateChange.bind(this),
        orientation: DeviceOrientation.PORTRAIT,
        onImageData: this.onImageData.bind(this),
      };

      const faceSession = await monitor.createFaceSession(options);
      console.log('Session created');
      this.session = faceSession;
      this.error.next({ code: -1 });
    } catch (e) {
      const error = e as { errorCode: number };
      this.error.next({ code: error.errorCode });
      console.error('Error creating a session', e);
    }
  }

  startMeasuring() {
    if (this.sessionState.value === SessionState.ACTIVE) {
      this.session!.start();
      this.error.next({ code: -1 });
    }
  }

  stopMeasuring() {
    if (this.sessionState.value === SessionState.MEASURING) {
      this.session!.stop();
    }
  }

  private onVitalSign(vitalSign: VitalSigns) {
    console.log('Vital sign:', vitalSign);
    this.updateVitalSigns(vitalSign);
  }

  private onFinalResults(vitalSignsResults: VitalSignsResults) {
    this.vitalSigns.next(null);
    this.updateVitalSigns(vitalSignsResults.results);
  }

  private onError(errorData: AlertData) {
    this.error.next(errorData);
  }

  private onWarning(warningData: AlertData) {
    if (
      warningData.code ===
      HealthMonitorCodes.MEASUREMENT_CODE_MISDETECTION_DURATION_EXCEEDS_LIMIT_WARNING
    ) {
      this.vitalSigns.next(null);
    }
    this.warning.next(warningData);
  }

  private onStateChange(state: SessionState) {
    this.sessionState.next(state);
    if (state === SessionState.MEASURING) {
      this.vitalSigns.next(null);
    }
  }

  private onEnabledVitalSigns(vitalSigns: EnabledVitalSigns) {
    this.enabledVitalSigns.next(vitalSigns);
  }

  private onOfflineMeasurement(offlineMeasurements: OfflineMeasurements) {
    this.offlineMeasurements.next(offlineMeasurements);
  }


  private onImageData(imageValidity: ImageValidity) {
    let message: string;
    if (imageValidity != ImageValidity.VALID) {
      switch (imageValidity) {
        case ImageValidity.INVALID_DEVICE_ORIENTATION:
          message = 'Unsupported Orientation';
          break;
        case ImageValidity.TILTED_HEAD:
          message = 'Head Tilted';
          break;
        case ImageValidity.FACE_TOO_FAR:
          message = 'You Are Too Far';
          break;
        case ImageValidity.UNEVEN_LIGHT:
          message = 'Uneven Lighting';
          break;
        case ImageValidity.INVALID_ROI:
        default:
          message = 'Face Not Detected';
      }
      this.info.next({
        type: InfoType.INSTRUCTION,
        message: message,
      });
    } else {
      this.setInfoWithDismiss({ type: InfoType.NONE });
    }
  }

  private setInfoWithDismiss(info: InfoData, seconds?: number) {
    if (!this.isDismissing) {
      this.info.next(info);
      if (seconds) {
        this.isDismissing = true;
        setTimeout(() => {
          this.info.next({ type: InfoType.NONE });
          this.isDismissing = false;
        }, seconds * 1000);
      }
    }
  }

  private updateVitalSigns(vitalSigns: VitalSigns) {
    this.vitalSigns.next({
      ...this.vitalSigns.value,
      ...vitalSigns,
    });
  }

  getVitalSigns() {
    return {
      pulseRate: {
        value: this.vitalSigns.value?.pulseRate?.value,
        isEnabled: this.enabledVitalSigns.value?.isEnabledPulseRate,
      },
      respirationRate: {
        value: this.vitalSigns.value?.respirationRate?.value,
        isEnabled: this.enabledVitalSigns.value?.isEnabledRespirationRate,
      },
      stress: {
        value: this.vitalSigns.value?.stressLevel?.value,
        isEnabled: this.enabledVitalSigns.value?.isEnabledStressLevel,
      },
      hrvSdnn: {
        value: this.vitalSigns.value?.sdnn?.value,
        isEnabled: this.enabledVitalSigns.value?.isEnabledSdnn,
      },
      spo2: {
        value: null,
        isEnabled: false,
      },
      bloodPressure: {
        value: this.vitalSigns.value?.bloodPressure?.value,
        isEnabled: this.enabledVitalSigns.value?.isEnabledBloodPressure,
      },
    };
  }
}