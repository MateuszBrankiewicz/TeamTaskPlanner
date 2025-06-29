import { Component } from '@angular/core';
import {FormInputComponent} from '../../../shared/form-input/form-input.component';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {AuthServiceService} from '../../service/auth-service.service';
import { firstValueFrom } from 'rxjs';
import { MatCard, MatCardActions, MatCardContent, MatCardHeader } from '@angular/material/card';
import {  MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, MatCard, FormInputComponent, MatCardHeader, MatCardContent, MatInputModule, MatFormFieldModule, MatButtonModule, FormInputComponent],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
  loginForm = new FormGroup({
  email : new FormControl('', [Validators.required, Validators.email]),
  password: new FormControl('', Validators.required)
});
  constructor(private authService: AuthServiceService, private router: Router) {
  }
  formSubitted = false;
  showError = false;
 async handleLogin() {
  this.formSubitted = true;
  if(this.loginForm.valid) {
    if(this.loginForm.value.email && this.loginForm.value.password) {
      const email = this.loginForm.value.email;
      const password = this.loginForm.value.password;
      try {
        const response = await firstValueFrom(
          this.authService.loginUser({email, password})
        );
        console.log(response);
        // ✅ Nawigacja TYLKO gdy login się powiedzie
        await this.router.navigate(['/']);
      }
      catch(err) {
        this.showError = true;
        console.log(err);
        // ✅ Tutaj nie ma nawigacji - zostaniesz na stronie logowania
      }
    }
  } else {
    this.loginForm.markAllAsTouched();
  }
}
  get emailControl(): FormControl {
    return this.loginForm.get('email') as FormControl;
  }

  get passwordControl(): FormControl {
    return this.loginForm.get('password') as FormControl;
  }
}
