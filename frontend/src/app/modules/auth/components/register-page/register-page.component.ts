import { Component } from '@angular/core';
import {FormInputComponent} from '../../../shared/form-input/form-input.component';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';
import {Router, RouterLink} from '@angular/router';
import {LoginServiceService} from '../../service/login-service.service';
import { firstValueFrom } from 'rxjs';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-register-page',
  imports: [
    FormInputComponent,

    ReactiveFormsModule,
    RouterLink,
    MatCardModule
  ],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss'
})
export class RegisterPageComponent {
  constructor(  private authService : LoginServiceService, private router: Router) {
  }
  registerForm: FormGroup = new FormGroup({
    email : new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    passwordConfirmation: new FormControl('', [Validators.required]),
  });
  formSubitted = false;
  passwordEqals : boolean = true;
  showError = false;
  emailTaken = false;
  async handleSubmit() {
    this.formSubitted = true;
    if (!this.registerForm.valid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    if( !(this.registerForm.controls['passwordConfirmation'].value === this.registerForm.controls['password'].value)) {
        this.passwordEqals = false;
        return;
    }
    this.passwordEqals = true;
    const email = this.registerForm.controls['email'].value;
    const password = this.registerForm.controls['password'].value;
    try{
      const response = await firstValueFrom(
        this.authService.registerUser({email,password})
      )
      console.log(response);
      await this.router.navigate(['/login']);

    }catch(err:any){
      if(err.status === 400){
        const errorData = err.error;

      if (errorData.errorCode === 'EMAIL_ALREADY_EXISTS') {
        this.registerForm.get('email')?.setErrors({
          emailTaken: true
        });
        this.emailTaken =true;
      }
      else{
      this.showError = true;
      }
    }
  }
  }
    get EmailControl() : FormControl {
    return this.registerForm.get('email') as FormControl;
  }
  get passwordControl() : FormControl {
    return this.registerForm.get('password') as FormControl;
  }
  get passwordConfirmationControl() : FormControl {
    return this.registerForm.get('passwordConfirmation') as FormControl;
  }
  get getPasswordEquals() : boolean {
    return this.passwordEqals;
  }
}
