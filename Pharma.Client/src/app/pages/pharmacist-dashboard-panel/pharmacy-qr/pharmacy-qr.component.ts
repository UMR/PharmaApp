import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { BinahScanService } from '../../../service/binah-scan-service.service';
import { PharmacyService } from '../../../service/pharmacy.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FileUpload } from 'primeng/fileupload';
import { Observable, Subscription } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-pharmacy-qr',
  standalone: false,
  templateUrl: './pharmacy-qr.component.html',
  styleUrl: './pharmacy-qr.component.css'
})
export class PharmacyQrComponent implements OnInit {

  constructor(private binahScanService: BinahScanService, private pharmaService: PharmacyService, private fb: FormBuilder, private sanitizer: DomSanitizer) { }

  qrCodeImage: any;
  user: any;
  displayModal: boolean = false;
  pharmacy: any;
  pharmacyRegistrationForm: FormGroup | any;
  public previewUrl: any = null;
  public fileName: any = null;
  public fileType = '.jpg, .png, .jpeg';
  public fileSizeError: boolean = false;
  public maxFileSize = 10485760;
  public userDocument: any = [];
  public imageData: any = '';
  isFileSelected: boolean = false;
  file: File | null = null;
  fileTypeOptions: any[] = [];
  splitterLayout: string = 'horizontal';
  @ViewChild('fileUpload')
  fileUpload!: FileUpload;
  chipInput: any;
  resizeObservable$: Observable<Event> | any;
  resizeSubscription$: Subscription | any;


  ngOnInit(): void {
    this.getPharmacyUser();
    this.getPharmacy();
    this.getQrCode();
  }

  getPharmacy() {
    this.pharmaService.getPharmacy().subscribe({
      next: (res) => {
        this.pharmacy = res.body;
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  getPharmacyUser() {
    this.pharmaService.getPharmacyUser().subscribe({
      next: (res) => {
        this.user = res.body;
        console.log(this.user);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  getQrCode() {
    this.binahScanService.getQrCode().subscribe({
      next: (res: any) => {
        console.log(res.status);
        this.qrCodeImage = `data:image/png;base64,${res.body?.base64String}`;
        console.log(this.qrCodeImage);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  showPharmacyRegistrationDialogue() {
    this.displayModal = true;
    this.updateSplitterLayout(window.innerWidth);
    this.initializaePharmacyRegistrationForm();
  }

  initializaePharmacyRegistrationForm() {
    this.pharmacyRegistrationForm = this.fb.group({
      storeName: [null],
      storeLogo: [null],
      storeAddress: [null],
      storeAddress2: [null],
    });
  }
  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    const width = (event.target as Window).innerWidth;
    this.updateSplitterLayout(width);
  }

  updateSplitterLayout(width: number) {
    this.splitterLayout = width >= 1024 ? 'horizontal' : 'vertical';
  }

  cancelRegistration() {

    this.pharmacyRegistrationForm.reset();
    this.displayModal = false;
    this.previewUrl = null;
  }
  updatePharmacyUser() {
    const requestModel = {
      storeName: this.pharmacyRegistrationForm.value.storeName,
      // storeLogo: this.previewUrl ? this.previewUrl.split(',')[1] : null,
      storeLogo: this.file,
      addressLine1: this.pharmacyRegistrationForm.value.storeAddress,
      addressLine2: this.pharmacyRegistrationForm.value.storeAddress2
    }
    this.binahScanService.pharmacyRegistration(requestModel).subscribe({
      next: (res) => {
        this.displayModal = false;
        this.getPharmacyUser();
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  onFileSelected(event: any) {
    const file = event.currentFiles[0];
    this.totalSize = file.size;
    this.totalSizePercent = (this.totalSize / 1000000) * 100;
    if (file) {
      this.file = file;
      this.fileName = file.name;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.previewUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
      this.isFileSelected = true;
    }
  }

  totalSize: number = 0;

  totalSizePercent: number = 0;


  choose(event: any, callback: () => void) {
    callback();
  }

  onRemoveTemplatingFile(event: any, file: any, removeFileCallback: (arg0: any, arg1: any) => void, index: any) {
    removeFileCallback(event, index);
    this.totalSize = 0;
    this.totalSizePercent = 0;
    this.file = null;
    this.pharmacyRegistrationForm.controls['importUrl'].setValue(null);
  }

  onClearTemplatingUpload(clear: () => void) {
    clear();
    this.totalSize = 0;
    this.totalSizePercent = 0;
    this.file = null;
    this.pharmacyRegistrationForm.controls['importUrl'].setValue(null);
  }

  uploadEvent(callback: () => void) {
    callback();
  }

  formatSize(bytes: number) {
    const k = 1024;
    const dm = 2;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    if (bytes === 0) return `0 Bytes`;
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return `${parseFloat((bytes / Math.pow(k, i)).toFixed(dm))} ${sizes[i]}`;
  }

  getFileUrl(arg0: File) {
    if (arg0) {
      return this.sanitizer.bypassSecurityTrustUrl((window.URL.createObjectURL(arg0)));
    }
    else {
      return null;
    }
  }


}
