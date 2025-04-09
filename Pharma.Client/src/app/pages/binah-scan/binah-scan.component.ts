import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MonitorService } from '../../service/monitor.service';
import { Router } from '@angular/router';
import { AuthService } from '../../service/auth.service';
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
import { ToastMessageService } from '../../service/toast-message.service';
export enum InfoType {
  NONE = 'NONE',
  INSTRUCTION = 'INSTRUCTION',
}

export interface InfoData {
  type: InfoType;
  message?: string;
}

@Component({
  selector: 'app-binah-scan',
  templateUrl: './binah-scan.component.html',
  standalone: false,
  styleUrls: ['./binah-scan.component.css'],
})
export class BinahScanComponent implements OnInit {
  isStarted = false;
  processingTime?: number;
  licenseKey?: string;
  session: HealthMonitorSession | null = null;
  sessionState = new BehaviorSubject<SessionState | null>(null);
  vitalSigns = new BehaviorSubject<any>({});
  info = new BehaviorSubject<InfoData>({ type: InfoType.NONE });
  warning = new BehaviorSubject<AlertData | null>(null);
  error = new BehaviorSubject<AlertData | null>(null);
  enabledVitalSigns = new BehaviorSubject<EnabledVitalSigns | null>(null);
  offlineMeasurements = new BehaviorSubject<OfflineMeasurements | null>(null);
  time: any;
  measurementStarted = false;

  isDismissing = false;

  @ViewChild('videoElement') videoElement!: ElementRef<HTMLVideoElement>;

  constructor(
    private monitorService: MonitorService,
    private authService: AuthService,
    private router: Router, private toastService: ToastMessageService) { }

  ngOnInit(): void { }

  async startScan() {
    this.isStarted = true;
    await this.startMonitorInitiation();
  }

  async startMonitorInitiation() {
    try {
      this.monitorService.getProcessingTime().subscribe((time: number | undefined) => this.processingTime = time);
      this.monitorService.getLicenseKey().subscribe((key: string | undefined) => this.licenseKey = key);

      if (!this.licenseKey || !this.processingTime) {
        console.error('License key or processing time not received.');
        return;
      }

      await this.initializeMonitor(this.licenseKey, '');
    } catch (e) {
      console.error('Error initializing monitor:', e);
    }
  }

  async initializeMonitor(licenseKey: string, productId: string) {
    try {
      console.log('Initializing monitor...');
      await monitor.initialize({ licenseKey });

      if (this.session && this.session.getState() === SessionState.ACTIVE) {
        this.session.terminate();
      }

      const options: FaceSessionOptions = {
        input: this.videoElement.nativeElement,
        cameraDeviceId: '',
        processingTime: this.processingTime ?? 60,
        onVitalSign: this.VitalSign.bind(this),
        onFinalResults: this.FinalResults.bind(this),
        onError: this.Error.bind(this),
        onWarning: this.Warning.bind(this),
        onStateChange: this.StateChange.bind(this),
        orientation: DeviceOrientation.PORTRAIT,
        onImageData: this.ImageData.bind(this),
      };

      this.session = await monitor.createFaceSession(options);
      console.log('Session created:', this.session);

    } catch (e) {
      console.error('Error initializing monitor session:', e);
    }
  }


  startMeasuring() {
    try {
      if (this.session && this.sessionState.value === SessionState.ACTIVE) {
        this.session.start();
        this.measurementStarted = true;
        if (this.time) {
          clearInterval(this.time);
        }

        this.time = setInterval(() => {
          if (this.processingTime! > 0) {
            this.processingTime!--;
          } else {
            clearInterval(this.time);
            console.log('Timer ended.');
          }
        }, 1000);
      } else {
        console.error('already started.');
      }
    }
    catch {
      this.measurementStarted = false;
      this.toastService.showError('Error', 'Error starting measurement:');
    }

  }
  stopMeasuring() {
    if (this.session && this.session.getState() === SessionState.MEASURING) {
      this.session.stop();
    }
  }

  VitalSign(vitalSign: VitalSigns) {
    console.log('Vital sign:', vitalSign);
    this.updateVitalSigns(vitalSign);
  }

  FinalResults(vitalSignsResults: VitalSignsResults) {
    console.log('Final results:', vitalSignsResults);
    this.vitalSigns.next(null);
    this.updateVitalSigns(vitalSignsResults.results);
  }

  Error(errorData: AlertData) {
    console.log('Error:', errorData);
    this.error.next(errorData);
  }

  Warning(warningData: AlertData) {
    if (
      warningData.code ===
      HealthMonitorCodes.MEASUREMENT_CODE_MISDETECTION_DURATION_EXCEEDS_LIMIT_WARNING
    ) {
      console.log('Warning: Measurement duration exceeds limit');
      this.vitalSigns.next(null);
    }
    console.log('Warning:', warningData);
    this.warning.next(warningData);
  }

  StateChange(state: SessionState) {
    console.log('State:', state);
    this.sessionState.next(state);
  }

  FaceDetected(faceDetected: boolean) {
    console.log('Face detected:', faceDetected);
  }

  ImageData(imageValidity: ImageValidity) {
    console.log('Image validity:', imageValidity);
    let message: string;
    if (imageValidity !== ImageValidity.VALID) {
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
        default:
          message = 'Face Not Detected';
      }
      this.info.next({ type: InfoType.INSTRUCTION, message });
      console.log(this.info.value);
    } else {
      this.setInfoWithDismiss({ type: InfoType.NONE });
    }
  }

  onEnabledVitalSigns(vitalSigns: EnabledVitalSigns) {
    this.enabledVitalSigns.next(vitalSigns);
  }

  onOfflineMeasurement(offlineMeasurements: OfflineMeasurements) {
    this.offlineMeasurements.next(offlineMeasurements);
  }

  setInfoWithDismiss(info: InfoData, seconds?: number) {
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

  updateVitalSigns(vitalSigns: VitalSigns) {
    const updated = {
      ...this.vitalSigns.value,
      ...vitalSigns,
    };
    this.vitalSigns.next(updated);
  }



  onLogoutClick() {
    this.stopMeasuring();
    this.session?.terminate();
    this.authService.logOut();
    this.router.navigate(['/pharmacy-login']);
  }

  getSessionState() {
    return this.sessionState.value
  }
}
