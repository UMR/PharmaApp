import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

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
  constructor(public sanitizer: DomSanitizer) {
    this.sanitizer = sanitizer;
  }
  youtubeVideoLink: string = 'https://umrtest.com/binah/';
  getLink() {
    return this.sanitizer.bypassSecurityTrustResourceUrl(this.youtubeVideoLink);
  }
}
