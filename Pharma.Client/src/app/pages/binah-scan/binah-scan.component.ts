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
} from '@biosensesignal/web-sdk';
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
  infoMessage: any;

  vitalCardsTop: any[] = [];
  vitalCardsBottom: any[] = [];
  hasFinalResults: boolean = false;

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
      console.log('Processing time:', this.processingTime);
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
    this.monitorService.getProcessingTime().subscribe(time => this.processingTime = time);
  }

  private clearTimer(): void {
    if (this.timeInterval) {
      clearInterval(this.timeInterval);
      this.timeInterval = null;
    }
  }

  private onVitalSign(vitalSign: VitalSigns): void {
    this.updateVitalSigns(vitalSign);
  }

  private onFinalResults(results: VitalSignsResults): void {
    this.vitalSigns$.next(null);
    this.hasFinalResults = true;
    this.updateVitalSigns(results.results);
  }

  private onError(error: AlertData): void {
    console.error('Error:', error);
    this.error$.next(error);
    this.setInfoWithDismiss({ type: InfoType.INSTRUCTION, message: 'Error occurred. Please try again.' }, 5);
    this.toastService.showError('Error', this.getErrorWithCode(error.code));

    this.stopMeasuring();
  }

  getErrorWithCode(code: number): string {
    const HealthMonitorCodeMessages: Record<number, string> = {
      [HealthMonitorCodes.DEVICE_CODE_MINIMUM_OS_VERSION_ERROR]: `"Minimum OS version not supported.${code}"`,
      [HealthMonitorCodes.DEVICE_CODE_INTERNAL_ERROR_1]: `"Internal device error 1.${code}"`,
      [HealthMonitorCodes.DEVICE_CODE_INTERNAL_ERROR_2]: "Internal device error 2.",
      [HealthMonitorCodes.DEVICE_CODE_INTERNAL_ERROR_3]: "Internal device error 3.",
      [HealthMonitorCodes.DEVICE_CODE_INTERNAL_ERROR_4]: "Internal device error 4.",
      [HealthMonitorCodes.DEVICE_CODE_CLOCK_SKEW_ERROR]: "Device clock skew detected.",
      [HealthMonitorCodes.DEVICE_CODE_MINIMUM_BROWSER_VERSION_ERROR]: "Minimum browser version not supported.",
      [HealthMonitorCodes.CAMERA_CODE_NO_CAMERA_ERROR]: "No camera found on the device.",
      [HealthMonitorCodes.CAMERA_CODE_CAMERA_OPEN_ERROR]: "Failed to open camera.",
      [HealthMonitorCodes.CAMERA_CODE_CAMERA_MISSING_PERMISSIONS_ERROR]: "Camera permission is missing.",
      [HealthMonitorCodes.CAMERA_CODE_UNEXPECTED_IMAGE_DIMENSIONS_WARNING]: "Unexpected image dimensions.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_18]: "License internal error 18.",
      [HealthMonitorCodes.LICENSE_CODE_ACTIVATION_LIMIT_REACHED_ERROR]: "License activation limit reached.",
      [HealthMonitorCodes.LICENSE_CODE_METER_ATTRIBUTE_USES_LIMIT_REACHED_ERROR]: "Meter attribute usage limit reached.",
      [HealthMonitorCodes.LICENSE_CODE_AUTHENTICATION_FAILED_ERROR]: "License authentication failed.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_1]: "License internal error 1.",
      [HealthMonitorCodes.LICENSE_CODE_INVALID_LICENSE_KEY_ERROR]: "Invalid license key.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_2]: "License internal error 2.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_3]: "License internal error 3.",
      [HealthMonitorCodes.LICENSE_CODE_REVOKED_LICENSE_ERROR]: "License has been revoked.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_4]: "License internal error 4.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_5]: "License internal error 5.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_7]: "License internal error 7.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_8]: "License internal error 8.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_9]: "License internal error 9.",
      [HealthMonitorCodes.LICENSE_CODE_LICENSE_EXPIRED_ERROR]: "License has expired.",
      [HealthMonitorCodes.LICENSE_CODE_LICENSE_SUSPENDED_ERROR]: "License has been suspended.",
      [HealthMonitorCodes.LICENSE_CODE_TOKEN_EXPIRED_ERROR]: "Token expired.",
      [HealthMonitorCodes.LICENSE_CODE_DEVICE_DEACTIVATED_ERROR]: "Device deactivated.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_10]: "License internal error 10.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_11]: "License internal error 11.",
      [HealthMonitorCodes.LICENSE_CODE_NETWORK_ISSUES_ERROR]: "Network issues occurred.",
      [HealthMonitorCodes.LICENSE_CODE_SSL_HANDSHAKE_ERROR]: "SSL handshake failed.",
      [HealthMonitorCodes.LICENSE_CODE_INTERNAL_ERROR_16]: "License internal error 16.",
      [HealthMonitorCodes.LICENSE_CODE_INPUT_LICENSE_KEY_EMPTY_ERROR]: "License key input is empty.",
      [HealthMonitorCodes.LICENSE_CODE_INPUT_FINGERPRINT_EMPTY_ERROR]: "Device fingerprint is empty.",
      [HealthMonitorCodes.LICENSE_CODE_INPUT_PRODUCT_ID_ILLEGAL_ERROR]: "Illegal product ID.",
      [HealthMonitorCodes.LICENSE_CODE_CANNOT_OPEN_FILE_FOR_READ_ERROR]: "Cannot open file for reading.",
      [HealthMonitorCodes.LICENSE_CODE_MONTHLY_USAGE_TRACKING_REQUIRES_SYNC_ERROR]: "Monthly usage tracking requires sync.",
      [HealthMonitorCodes.LICENSE_CODE_SSL_HANDSHAKE_DEVICE_DATE_ERROR]: "Device date error during SSL handshake.",
      [HealthMonitorCodes.LICENSE_CODE_SSL_HANDSHAKE_CERTIFICATE_EXPIRED_ERROR]: "SSL certificate expired.",
      [HealthMonitorCodes.LICENSE_CODE_MIN_SDK_ERROR]: "Minimum SDK requirement not met.",
      [HealthMonitorCodes.LICENSE_CODE_MISSING_SDK_VERSION]: "Missing SDK version.",
      [HealthMonitorCodes.LICENSE_CODE_FORBIDDEN_LICENSE_TYPE_ERROR]: "Forbidden license type.",
      [HealthMonitorCodes.LICENSE_CODE_NETWORK_TIMEOUT_ERROR]: "Network timeout.",
      [HealthMonitorCodes.MEASUREMENT_CODE_MISDETECTION_DURATION_EXCEEDS_LIMIT_ERROR]: "Misdetection duration exceeded limit.",
      [HealthMonitorCodes.MEASUREMENT_CODE_INVALID_RECENT_DETECTION_RATE_ERROR]: "Invalid recent detection rate because of FPS error.",
      [HealthMonitorCodes.MEASUREMENT_CODE_LICENSE_ACTIVATION_FAILED_ERROR]: "License activation failed during measurement.",
      [HealthMonitorCodes.MEASUREMENT_CODE_INVALID_MEASUREMENT_AVERAGE_DETECTION_RATE_ERROR]: "Invalid average detection rate.",
      [HealthMonitorCodes.MEASUREMENT_CODE_TOO_MANY_FRAMES_INORDER_ERROR]: "Too many in-order frames.",
      [HealthMonitorCodes.MEASUREMENT_CODE_MISDETECTION_DURATION_EXCEEDS_LIMIT_WARNING]: "Misdetection duration warning.",
      [HealthMonitorCodes.MEASUREMENT_CODE_INVALID_RECENT_FPS_RATE_WARNING]: "Invalid recent FPS rate.",
      [HealthMonitorCodes.MEASUREMENT_CODE_MEASUREMENT_MISPLACED_FRAME_WARNING]: "Misplaced frame in measurement.",
      [HealthMonitorCodes.VITAL_SIGN_CODE_BLOOD_PRESSURE_PROCESSING_FAILED_WARNING]: "Blood pressure processing failed.",
      [HealthMonitorCodes.VITAL_SIGN_CODE_MEASURING_WITH_NO_ENABLED_VITAL_SIGNS_WARNING]: "No vital signs enabled.",
      [HealthMonitorCodes.SESSION_CODE_ILLEGAL_START_CALL_ERROR]: "Illegal session start call.",
      [HealthMonitorCodes.SESSION_CODE_ILLEGAL_STOP_CALL_ERROR]: "Illegal session stop call.",
      [HealthMonitorCodes.INITIALIZATION_CODE_INTERNAL_ERROR_1]: "Initialization internal error 1.",
      [HealthMonitorCodes.INITIALIZATION_CODE_INVALID_PROCESSING_TIME_ERROR]: "Invalid processing time.",
      [HealthMonitorCodes.INITIALIZATION_CODE_INVALID_LICENSE_FORMAT]: "Invalid license format.",
      [HealthMonitorCodes.INITIALIZATION_CODE_SDK_LOAD_FAILURE]: "Failed to load SDK.",
      [HealthMonitorCodes.INITIALIZATION_CODE_UNSUPPORTED_USER_WEIGHT]: "Unsupported user weight.",
      [HealthMonitorCodes.INITIALIZATION_CODE_UNSUPPORTED_USER_AGE]: "Unsupported user age.",
      [HealthMonitorCodes.INITIALIZATION_CODE_CONCURRENT_SESSIONS_ERROR]: "Concurrent sessions not allowed.",
      [HealthMonitorCodes.INITIALIZATION_CODE_UNSUPPORTED_USER_HEIGHT]: "Unsupported user height.",
      [HealthMonitorCodes.INITIALIZATION_CODE_MEMORY_ALLOCATION_ERROR]: "Memory allocation failed.",
      [HealthMonitorCodes.INITIALIZATION_CODE_INITIAL_MEMORY_ALLOCATION_ERROR]: "Initial memory allocation failed.",
      [HealthMonitorCodes.INITIALIZATION_CODE_BROWSER_NOT_SUPPORTING_SHARED_ARRAY_BUFFER_ERROR]: "Browser doesn't support SharedArrayBuffer.",
      [HealthMonitorCodes.INITIALIZATION_MEMORY_CONSUMPTION_WARNING]: "High memory consumption warning."
    };
    return HealthMonitorCodeMessages[code] || "Unknown error.";
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
      this.infoMessage = message;
    } else {
      this.setInfoWithDismiss({ type: InfoType.NONE });
      this.infoMessage = null;
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
