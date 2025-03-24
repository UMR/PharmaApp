import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../service/auth.service';
import { first } from 'rxjs';
import { ToastMessageService } from '../../../service/toast-message.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  userloginForm: FormGroup | any;
  currentDate: any;
  /**
   *
   */
  constructor(private router: Router, private fb: FormBuilder, private authService: AuthService, private route: ActivatedRoute, private toastService: ToastMessageService) {

  }
  ngOnInit(): void {
    this.initializeForm();
  }

  get f() { return this.userloginForm.controls; }

  login() {
    this.authService.login(this.userloginForm.value).pipe(
      first()
    ).subscribe({
      next: (res) => {
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.toastService.showSuccess('Success', 'Login Successful');
        this.router.navigateByUrl(returnUrl);
      },
      error: () => {
        console.log("error");
      }
    });
  }

  initializeForm() {
    this.userloginForm = this.fb.group({
      loginId: [null, Validators.required],
      pin: [null, Validators.required]
    });
  }
  register() {
    this.router.navigate(['/pharmacy-registration']);
  }
}

