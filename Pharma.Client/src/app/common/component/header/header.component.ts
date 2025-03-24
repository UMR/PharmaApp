import { Component, EventEmitter, Input, Output } from '@angular/core';
import { authCookieKey } from '../../constant/auth-key';
import { Router } from '@angular/router';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  @Output() sideNavToggled = new EventEmitter<boolean>();
  menuStatus: boolean = true;
  @Input() user: any;

  constructor(
    private router: Router, private confirmationService: ConfirmationService) {


  }
  ngOnInit() {

  }
  sideNavToggle() {
    this.menuStatus = !this.menuStatus;
    this.sideNavToggled.emit(this.menuStatus)
  }
  onLogoutClick() {
    this.confirm();

  }
  goHome() {
    this.router.navigate(['/']);
  }


  onSignOut() {
    localStorage.removeItem(authCookieKey);
    this.router.navigate(['/login']);
  }
  confirm() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to sign out?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      accept: () => {
        this.onSignOut();
      },
      reject: () => {
      }
    });
  }
  onNotificationClick() {
    console.log('Notification Clicked');
  }

}
