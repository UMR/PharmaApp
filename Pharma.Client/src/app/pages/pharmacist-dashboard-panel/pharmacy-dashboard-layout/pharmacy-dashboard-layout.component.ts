import { Component, HostListener, Input } from '@angular/core';
import { authCookieKey } from '../../../common/constant/auth-key';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pharmacy-dashboard-layout',
  standalone: false,
  templateUrl: './pharmacy-dashboard-layout.component.html',
  styleUrl: './pharmacy-dashboard-layout.component.css'
})
export class PharmacyDashboardLayoutComponent {
  constructor(private router: Router) { }
  @Input() sideNavStatus: boolean = true;
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

  }
}
