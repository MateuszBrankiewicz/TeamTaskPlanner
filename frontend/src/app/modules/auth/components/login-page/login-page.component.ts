import { Component, inject, signal } from '@angular/core';
import {FormInputComponent} from '../../../shared/form-input/form-input.component';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {LoginServiceService} from '../../service/login-service.service';
import { firstValueFrom } from 'rxjs';
import { MatCard, MatCardActions, MatCardContent, MatCardHeader } from '@angular/material/card';
import {  MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../../shared/services/auth.service';
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
  loginService = inject(LoginServiceService);
  authServce = inject(AuthService);
  constructor( private router: Router) {
  }
  formSubitted = false;
  showError = signal(false);
 async handleLogin() {
  this.formSubitted = true;
  if(this.loginForm.valid) {
    this.showError.set(false);
    if(this.loginForm.value.email && this.loginForm.value.password) {
      const email = this.loginForm.value.email;
      const password = this.loginForm.value.password;
      try {
        const response = await firstValueFrom(
          this.loginService.loginUser({email, password})
        );
        console.log(response);
        this.authServce.authUser.set({userName:response.email, isLoggedIn:true})
        await this.router.navigate(['/']);
      }
      catch(err) {
        this.showError.set(true);
        console.log(err);
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
