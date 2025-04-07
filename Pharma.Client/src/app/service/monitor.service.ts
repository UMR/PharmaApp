import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MonitorService {

  constructor() { }
  getProcessingTime(): Observable<number> {
    return of(60);
  }

  getLicenseKey(): Observable<string> {
    return of('C5A15F-F168F2-4633BE-BFF65F-E238FB-43CD12');
  }
}