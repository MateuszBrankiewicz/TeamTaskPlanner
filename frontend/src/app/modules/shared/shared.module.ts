import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import {ButtonComponent} from './button/button.component';
import { ThemeToggleComponent } from './theme-toggle/theme-toggle.component';
import { NavComponent } from './nav/nav.component';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ThemeToggleComponent,
    NavComponent
  ],
  exports: [ThemeToggleComponent, NavComponent]
})
export class SharedModule { }
