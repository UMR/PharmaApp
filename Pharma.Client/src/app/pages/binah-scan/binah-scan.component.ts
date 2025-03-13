// binah-scan.component.ts
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MonitorService } from '../../service/monitor.service';
import { SessionState } from '@binah/web-sdk';
import { VitalSign, VitalSignsResults } from '@binah/web-sdk/dist/common/types';
import { AuthenticationService } from '../../service/authentication.service';
import { PharmacyMerchantService } from '../../service/pharmacy-merchant.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-binah-scan',
  templateUrl: './binah-scan.component.html',
  standalone: false,
  styleUrls: ['./binah-scan.component.css'],
})
export class BinahScanComponent implements OnInit {

  isStarted: boolean = false;
  processingTime: number | undefined;
  licenseKey: string | undefined;
  sessionState: any;
  cameraId: string | undefined;
  vitals: VitalSignsResults | null = null;

  @ViewChild('videoElement', { static: false }) videoElement!: ElementRef;

  constructor(private monitorService: MonitorService, private authService: PharmacyMerchantService, private router: Router) { }

  ngOnInit(): void {
    this.getCameraDevices();

    this.monitorService.vitalSigns$.subscribe((vitals) => {
      this.vitals = vitals as VitalSignsResults;
      console.log('Vitals:', vitals);
    });
  }

  startScan() {
    console.log('Starting scan');
    this.isStarted = true;
    this.startMonitorInitiation();
  }

  async startMonitorInitiation() {
    this.monitorService.getProcessingTime().subscribe((time: number | undefined) => this.processingTime = time);
    this.monitorService.getLicenseKey().subscribe((key: string | undefined) => this.licenseKey = key);

    try {
      await this.monitorService.initializeMonitor(this.licenseKey as string, "");
      this.monitorService.isMonitorReady$.subscribe((isReady: boolean) => {
        if (isReady) {
          this.startCamera();
        }
      });
    } catch (e) {
      console.error('Error initializing monitor:', e);
    }
  }

  startCamera() {
    if (this.licenseKey && this.videoElement && this.cameraId) {
      navigator.mediaDevices
        .getUserMedia({ video: { deviceId: { exact: this.cameraId } } })
        .then((stream) => {
          this.videoElement.nativeElement.srcObject = stream;
          this.videoElement.nativeElement.play();
        })
        .catch((err) => console.error('Error accessing camera:', err));
    } else {
      console.error('Camera ID or video element is not available.');
    }
  }

  getCameraDevices() {
    navigator.mediaDevices.enumerateDevices()
      .then(devices => {
        const videoDevices = devices.filter(device => device.kind === 'videoinput');
        if (videoDevices.length > 0) {
          this.cameraId = videoDevices[0].deviceId;
          console.log('Camera ID:', this.cameraId);
        } else {
          console.error('No camera devices found.');
        }
      })
      .catch(err => {
        console.error('Error fetching devices:', err);
      });
  }

  async startMeasuring() {
    if (this.cameraId && this.processingTime && this.videoElement) {
      await this.monitorService.createSession(this.videoElement.nativeElement, this.cameraId, this.processingTime);
      this.monitorService.startMeasuring();
    } else {
      console.error('Camera ID, processing time, or video element is not available.');
    }
  }

  onLogoutClick() {
    this.stopCamera();
    this.monitorService.stopMeasuring();
    this.authService.logOut();
    this.router.navigate(['/pharmacy-login']);
  }


  stopCamera() {
    if (this.videoElement && this.videoElement.nativeElement.srcObject) {
      let stream = this.videoElement.nativeElement.srcObject as MediaStream;
      let tracks = stream.getTracks();

      tracks.forEach(track => {
        track.stop()
      });

      this.videoElement.nativeElement.srcObject = null;
    }
  }


}