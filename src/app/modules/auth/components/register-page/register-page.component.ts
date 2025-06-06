import { Component } from '@angular/core';
import {FormInputComponent} from '../../../shared/form-input/form-input.component';
import {ButtonComponent} from '../../../shared/button/button.component';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-register-page',
  imports: [
    FormInputComponent,
    ButtonComponent,
    ReactiveFormsModule,
    NgIf,
    RouterLink
  ],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss'
})
export class RegisterPageComponent {
  registerForm: FormGroup = new FormGroup({
    email : new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    passwordConfirmation: new FormControl('', [Validators.required]),
  });
  formSubitted = false;
  passwordEqals : boolean = true;
  handleSubmit() {
    this.formSubitted = true;
    if (!this.registerForm.valid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    if( !(this.passwordControl === this.passwordConfirmationControl)){
        this.passwordEqals = false;
        return;
    }
    this.passwordEqals = true;
    console.log(this.registerForm.value);
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
