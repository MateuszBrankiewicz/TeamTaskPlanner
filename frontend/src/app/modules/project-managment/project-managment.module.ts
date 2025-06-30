import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CreateProjectPageComponent } from './create-project-page/create-project-page.component';
import { ProjectRoutingModule } from './project-routing.module';



@NgModule({
  declarations: [],
  imports: [
  CommonModule,
  ProjectRoutingModule
  ]
})
export class ProjectManagmentModule {}
