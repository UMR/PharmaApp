import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { ToastMessageService } from '../../../service/toast-message.service';
import { AuthService } from '../../../service/auth.service';
import { RoleType } from '../../model/role.model';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {

  constructor(
    private router: Router,
    private confirmationService: ConfirmationService,
    private toastService: ToastMessageService,
    private authService: AuthService,
  ) { }

  @Input() sideNavStatus: boolean = true;
  userRole: string = '';
  filteredMenu: any[] = [];

  menu = [
    {
      number: '1',
      name: 'Dashboard',
      icon: 'fa fa-home',
      routerLink: '/',
      roles: [],
    },
    {
      number: '2',
      name: 'Pharmacy QR',
      icon: 'fa fa-qrcode',
      routerLink: '/pharmacy-qr',
      roles: [RoleType.PHARMACIST],
    },
    {
      number: '3',
      name: 'Binah Scan',
      icon: 'fa fa-camera',
      routerLink: '/pay-now',
      roles: [],
    },
    {
      number: '4',
      name: 'Scan Log',
      icon: 'fa fa-history',
      routerLink: '/scan-log',
      roles: [RoleType.PHARMACIST],
    },
    {
      number: '20',
      name: 'Log Out',
      icon: 'fa fa-sign-out',
      routerLink: null,
      roles: [],
    },

  ];

  ngOnInit(): void {
    this.filteredMenu = this.menu;
  }

  getUserRoles(): string[] {
    return [''];
  }


  navigateTo(route: string, menuItemName: string) {
    if (menuItemName === 'Log Out') {
      this.confirm();
    }
    else {
      this.router.navigate([route]);
    }
  }

  isActive(route: string, menuItemName: string): boolean {
    if (route === '/' && menuItemName === 'Dashboard') {
      return this.router.url === '/';
    }
    if (route === '/' && menuItemName === 'Log Out') {
      return false;
    }
    return this.router.url === route || this.router.url.includes(route);
  }

  confirm(): void {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to sign out?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: 'none',
      rejectIcon: 'none',
      rejectButtonStyleClass: 'p-button-text',
      accept: () => {
        this.onSignOut();
      },
      reject: () => {
        this.router.navigate(['/']);
      },
    });
  }

  onSignOut(): void {
    this.authService.logOut();
    this.router.navigate(['/pharmacy-login']);
    this.toastService.showSuccess(
      'Success',
      'You have been signed out successfully'
    );
  }

}
