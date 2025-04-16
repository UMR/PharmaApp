import { Routes } from '@angular/router';
import { BinahScanComponent } from './binah-scan/binah-scan.component';

export const routes: Routes = [
    {
        path: '',
        component: BinahScanComponent
    },
    {
        path: "**",
        redirectTo: '/'
    }
];
