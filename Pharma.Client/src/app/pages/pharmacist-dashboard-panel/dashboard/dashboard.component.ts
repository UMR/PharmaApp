import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AuthService } from '../../../service/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  /**
   *
   */
  Roles: any[] = [];
  constructor(public sanitizer: DomSanitizer, private authService: AuthService) {
    this.sanitizer = sanitizer;
    this.getRoles();
  }

  getRoles() {
    this.Roles = this.authService.getRoles();
    console.log(this.Roles);
  }

  youtubeVideoLink: string = 'https://umrtest.com/binah/';
  getLink() {
    return this.sanitizer.bypassSecurityTrustResourceUrl(this.youtubeVideoLink);
  }
}
