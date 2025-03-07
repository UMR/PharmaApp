import { Component } from '@angular/core';
import { PrimeNG } from 'primeng/config';
import { definePreset } from '@primeng/themes';
import Aura from '@primeng/themes/aura';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
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
      50: '{rose.50}',
      100: '{rose.100}',
      200: '{rose.200}',
      300: '{rose.300}',
      400: '{rose.400}',
      500: '{rose.500}',
      600: '{rose.600}',
      700: '{rose.700}',
      800: '{rose.800}',
      900: '{rose.900}',
      950: '{rose.950}'
    },
    colorScheme: {
      light: {
        primary: {
          color: '{rose.950}',
          inverseColor: '#ffffff',
          hoverColor: '{rose.900}',
          activeColor: '{rose.800}'
        },
        highlight: {
          background: '{rose.950}',
          focusBackground: '{rose.700}',
          color: '#ffffff',
          focusColor: '#ffffff'
        }
      }
    }
  }
});

