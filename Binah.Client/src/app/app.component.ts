import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { definePreset } from '@primeng/themes';
import { PrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(private primeng: PrimeNG) {
    this.primeng.theme.set({
      preset: MyPreset,
      options: {
        darkModeSelector: false || 'none',
        cssLayer: {
          name: 'primeng',
          order: 'bootstrap, primeng'
        }
      }
    })
  }

}
const MyPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{fuchsia.50}',
      100: '{fuchsia.100}',
      200: '{fuchsia.200}',
      300: '{fuchsia.300}',
      400: '{fuchsia.400}',
      500: '{fuchsia.500}',
      600: '{fuchsia.600}',
      700: '{fuchsia.700}',
      800: '{fuchsia.800}',
      900: '{fuchsia.900}',
      950: '{fuchsia.950}'
    }
  }
});
