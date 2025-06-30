import { Component, computed, inject, resource } from '@angular/core';
import { ProjectManagmentService } from '../project-managment.service';
import { firstValueFrom } from 'rxjs';
import { MatListModule } from '@angular/material/list';
import { CommonModule } from '@angular/common';
import { Projects } from '../project-types';
import { SingleListItemComponent } from "../../shared/single-list-item/single-list-item.component";
@Component({
  selector: 'app-show-projects',
  imports: [MatListModule, CommonModule, SingleListItemComponent],
  templateUrl: './show-projects.component.html',
  styleUrl: './show-projects.component.scss'
})
export class ShowProjectsComponent {
    projectService = inject(ProjectManagmentService);
    projectResource = resource ({
      loader: () => firstValueFrom(this.projectService.takeProjects())
    })
    projects = computed(()=> this.projectResource.value() ?? []);
    error = computed(() => this.projectResource.error());
}
