import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ButtonComponent} from './button/button.component';
import { ThemeToggleComponent } from './theme-toggle/theme-toggle.component';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ButtonComponent,
    ThemeToggleComponent
  ],
  exports: [ButtonComponent,ThemeToggleComponent]
})
export class SharedModule { }
