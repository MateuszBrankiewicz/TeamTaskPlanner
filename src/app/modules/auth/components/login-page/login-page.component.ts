import { Component } from '@angular/core';
import {ButtonComponent} from '../../../shared/button/button.component';
import {FormInputComponent} from '../../../shared/form-input/form-input.component';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';

@Component({
  selector: 'app-login-page',
  imports: [ButtonComponent, FormInputComponent, ReactiveFormsModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
  loginForm = new FormGroup({
  email : new FormControl('', [Validators.required, Validators.email]),
  password: new FormControl('', Validators.required)
});
  handleLogin() {
    if(this.loginForm.valid)
    {
      console.log(this.loginForm.value);
    }else{
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
