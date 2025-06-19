import { Component } from '@angular/core';
import {ButtonComponent} from '../../../shared/button/button.component';
import {FormInputComponent} from '../../../shared/form-input/form-input.component';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {AuthServiceService} from '../../service/auth-service.service';
import { firstValueFrom } from 'rxjs';

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
  constructor(private authService: AuthServiceService, private router: Router) {
  }
  formSubitted = false;
  showError = false;
 async  handleLogin() {
    this.formSubitted = true;
    if(this.loginForm.valid)
    {
      if(this.loginForm.value.email && this.loginForm.value.password) {
        const email = this.loginForm.value.email;
        const password = this.loginForm.value.password;
        try{
          const response = firstValueFrom(
            this.authService.loginUser({email,password})
          )
          console.log(response);
          await this.router.navigate(['/'])
        }
        catch(err){
          this.showError = true;
          console.log(err);
        }
      }
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
