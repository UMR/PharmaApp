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
  @ViewChild('videoElement') videoElement!: ElementRef<HTMLVideoElement>;

  isStarted = false;
  isDismissing = false;
  measurementStarted = false;

  session: HealthMonitorSession | null = null;

  processingTime?: number;
  licenseKey?: string;
  timeInterval: any;

  sessionState$ = new BehaviorSubject<SessionState | null>(null);
  vitalSigns$ = new BehaviorSubject<any>({});
  info$ = new BehaviorSubject<InfoData>({ type: InfoType.NONE });
  warning$ = new BehaviorSubject<AlertData | null>(null);
  error$ = new BehaviorSubject<AlertData | null>(null);
  enabledVitalSigns$ = new BehaviorSubject<EnabledVitalSigns | null>(null);
  offlineMeasurements$ = new BehaviorSubject<OfflineMeasurements | null>(null);
  isMeasuring: boolean = false;

  vitalCardsTop: any[] = [];
  vitalCardsBottom: any[] = [];

  constructor(
    private monitorService: MonitorService,
    private authService: AuthService,
    private router: Router,
    private toastService: ToastMessageService
  ) { }

  ngOnInit(): void { }

  async startScan(): Promise<void> {
    this.isStarted = true;
    this.monitorService.getProcessingTime().subscribe(time => this.processingTime = time);
    this.monitorService.getLicenseKey().subscribe(key => this.licenseKey = key);

    if (!this.licenseKey || !this.processingTime) {
      console.error('Missing license key or processing time.');
      return;
    }

    await this.initializeMonitor(this.licenseKey);
  }

  async initializeMonitor(licenseKey: string): Promise<void> {
    try {
      await monitor.initialize({ licenseKey });

      if (this.session?.getState() === SessionState.ACTIVE) {
        this.session.terminate();
      }

      const options: FaceSessionOptions = {
        input: this.videoElement.nativeElement,
        cameraDeviceId: '',
        processingTime: this.processingTime!,
        orientation: DeviceOrientation.PORTRAIT,
        onVitalSign: this.onVitalSign.bind(this),
        onFinalResults: this.onFinalResults.bind(this),
        onError: this.onError.bind(this),
        onWarning: this.onWarning.bind(this),
        onStateChange: this.onStateChange.bind(this),
        onImageData: this.onImageData.bind(this),
      };

      this.session = await monitor.createFaceSession(options);
    } catch (error) {
      console.error('Error initializing session:', error);
    }
  }

  startMeasuring(): void {
    if (!this.session || this.sessionState$.value !== SessionState.ACTIVE) {
      this.toastService.showError('Error', 'Session not active.');
      return;
    }

    this.session.start();
    this.measurementStarted = true;
    this.isMeasuring = true;
    this.clearTimer();

    this.timeInterval = setInterval(() => {
      if (this.processingTime! > 0) {
        this.processingTime!--;
      } else {
        this.stopMeasuring();
      }
    }, 1000);
  }

  stopMeasuring(): void {
    if (this.session?.getState() === SessionState.MEASURING) {
      this.session.stop();
    }
    this.clearTimer();
    this.measurementStarted = false;
    this.isMeasuring = false;
  }

  private clearTimer(): void {
    if (this.timeInterval) {
      clearInterval(this.timeInterval);
    }
  }

  private onVitalSign(vitalSign: VitalSigns): void {
    this.updateVitalSigns(vitalSign);
  }

  private onFinalResults(results: VitalSignsResults): void {
    this.vitalSigns$.next(null);
    this.updateVitalSigns(results.results);
  }

  private onError(error: AlertData): void {
    console.error('Error:', error);
    this.error$.next(error);
    this.setInfoWithDismiss({ type: InfoType.INSTRUCTION, message: 'Error occurred. Please try again.' }, 5);
    this.toastService.showError('Error', 'Error occurred. Please try again.');
    this.stopMeasuring();
  }

  private onWarning(warning: AlertData): void {
    this.warning$.next(warning);
    if (warning.code === HealthMonitorCodes.MEASUREMENT_CODE_MISDETECTION_DURATION_EXCEEDS_LIMIT_WARNING) {
      this.vitalSigns$.next(null);
    }
  }

  private onStateChange(state: SessionState): void {
    this.sessionState$.next(state);
  }

  private onImageData(imageValidity: ImageValidity): void {
    let message = '';
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

    if (imageValidity !== ImageValidity.VALID) {
      this.info$.next({ type: InfoType.INSTRUCTION, message });
    } else {
      this.setInfoWithDismiss({ type: InfoType.NONE });
    }
  }

  private updateVitalSigns(vitalSigns: VitalSigns): void {
    const updated = { ...this.vitalSigns$.value, ...vitalSigns };
    this.vitalSigns$.next(updated);

    this.vitalCardsTop = [
      { label: 'Heart Rate', value: updated.pulseRate?.value, unit: 'BPM', icon: 'fa fa-heart' },
      { label: 'Oxygen Saturation', value: updated.oxygenSaturation?.value, unit: 'SpO2%', icon: 'fa fa-tint' },
      { label: 'Respiration', value: updated.respirationRate?.value, unit: 'RPM', icon: 'fa fa-stethoscope' },
    ];

    this.vitalCardsBottom = [
      { label: 'Hemoglobin', value: updated.hemoglobin?.value, unit: 'g/dL', icon: 'fa fa-area-chart' },
      { label: 'Stress Level', value: updated.stressLevel?.value, unit: 'High/Low', icon: 'fa fa-deafness' },
      { label: 'Blood Pressure', value: `${updated.bloodPressure?.systolic ?? ""}/${updated.bloodPressure?.diastolic ?? ""}`, unit: 'mmHg', icon: 'fa fa-heartbeat' },
    ];
  }

  setInfoWithDismiss(info: InfoData, seconds?: number): void {
    if (!this.isDismissing) {
      this.info$.next(info);
      if (seconds) {
        this.isDismissing = true;
        setTimeout(() => {
          this.info$.next({ type: InfoType.NONE });
          this.isDismissing = false;
        }, seconds * 1000);
      }
    }
  }

  onLogoutClick(): void {
    this.stopMeasuring();
    this.session?.terminate();
    this.authService.logOut();
    this.router.navigate(['/pharmacy-login']);
  }

  getSessionState(): SessionState | null {
    return this.sessionState$.value;
  }
}
