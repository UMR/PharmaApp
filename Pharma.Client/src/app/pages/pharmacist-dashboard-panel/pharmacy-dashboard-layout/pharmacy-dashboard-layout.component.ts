import { Component, HostListener, Input } from '@angular/core';
import { authCookieKey } from '../../../common/constant/auth-key';
import { Router } from '@angular/router';
import { PharmacyService } from '../../../service/pharmacy.service';

@Component({
  selector: 'app-pharmacy-dashboard-layout',
  standalone: false,
  templateUrl: './pharmacy-dashboard-layout.component.html',
  styleUrl: './pharmacy-dashboard-layout.component.css'
})
export class PharmacyDashboardLayoutComponent {
  constructor(private router: Router, private pharmacyService: PharmacyService) { }
  @Input() sideNavStatus: boolean = true;
  user: any;
  ngOnInit() {
    this.checkScreenSize();
    if (!localStorage.getItem(authCookieKey) || localStorage.getItem(authCookieKey) == null) {
      this.router.navigate(['/login']);
    }
    this.bindUserDetails();
  }
  @HostListener('window:resize', ['$event'])
  onResize() {
    this.checkScreenSize();
  }

  checkScreenSize() {
    const screenWidth = window.innerWidth;
    this.sideNavStatus = screenWidth > 768;
  }
  bindUserDetails() {
    this.pharmacyService.getCurrentUser().subscribe({
      next: (res) => {
        this.user = res.body;

      },
      error: () => {
        console.log("error");
      }
    })

  }
}
