import { Component, inject, input, signal } from '@angular/core';
import {FormControl, FormGroup,ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { FormInputComponent } from "../../shared/form-input/form-input.component";
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { ProjectManagmentService } from '../project-managment.service';
import { takeUntil } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DialogComponent } from '../../shared/dialog/dialog.component';

@Component({
  selector: 'app-create-project-page',
  imports: [SharedModule, FormInputComponent,ReactiveFormsModule, MatCardModule],
  templateUrl: './create-project-page.component.html',
  styleUrl: './create-project-page.component.scss'
})
export class CreateProjectPageComponent {
  projectService = inject(ProjectManagmentService);
  dialog = inject(MatDialog);
  router = inject(Router);
  showError = signal(false);
  createProjectForm :FormGroup = new FormGroup({
    projectName : new FormControl('', [Validators.required]),
    projectDescription : new FormControl('', [Validators.required])
})
onSubmit() {
  const name = this.createProjectForm.get('projectName')?.value;
  const description = this.createProjectForm.get('projectDescription')?.value;
  this.projectService.createProject({name:name, description:description}).subscribe({
    next: (response) => {
      const dialogRef = this.dialog.open(DialogComponent, {
        data: {
          title: 'Sukces',
          message: 'Projekt został utworzony pomyślnie!',
          showCancel: false
        }
      });

      dialogRef.afterClosed().subscribe(() => {
        this.router.navigate(['/project']);
      });
    },
    error: (error) => {
      this.showError.set(true);
      this.dialog.open(DialogComponent, {
        data: {
          title: 'Błąd',
          message: 'Wystąpił błąd podczas tworzenia projektu.',
          showCancel: false
        }
      });
    }
  })
}
get ProjectName() : FormControl{
  return this.createProjectForm.get('projectName') as FormControl;
}
get ProjectDescription() : FormControl{
  return this.createProjectForm.get('projectDescription') as FormControl;
}
}
