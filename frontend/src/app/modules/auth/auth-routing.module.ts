import {RouterModule, Routes} from '@angular/router';
import {LoginPageComponent} from './components/login-page/login-page.component';
import {NgModule} from '@angular/core';
import {RegisterPageComponent} from './components/register-page/register-page.component';
const routes: Routes = [
  {
    path: 'login',
    component: LoginPageComponent
  },
  {
    path: '',
    component: RegisterPageComponent
  }
]
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule {}
