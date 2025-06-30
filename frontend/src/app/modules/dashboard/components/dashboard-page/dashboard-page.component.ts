import { firstValueFrom } from 'rxjs';
import { Component, computed, effect, inject, resource, signal } from '@angular/core';
import { ThemeToggleComponent } from '../../../shared/theme-toggle/theme-toggle.component';
import { SharedModule } from "../../../shared/shared.module";
import { NavComponent } from "../../../shared/nav/nav.component";
import { MatCardModule } from '@angular/material/card';
import { DashboardService } from '../../dashboard.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

interface Task {
  id: number;
  title: string;
  description: string;
  createdDate: string;
  dueDate: string;
  priority: string;
  status: string;
}

@Component({
  imports: [CommonModule,MatCardModule, MatButtonModule],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.scss'
})

export class DashboardPageComponent {

  // tasks = signal<Task[]>([]);
  dashboardService = inject(DashboardService);
  tasksResource = resource({
    loader: () => firstValueFrom(this.dashboardService.getDashboard())
  });
  tasks = computed(() => this.tasksResource.value() ?? []);
  error = computed(() => this.tasksResource.error());
  changeTaskAsDone(task: Task){
    this.dashboardService.changeTaskAsDone(task)
    .pipe(takeUntilDestroyed())
    .subscribe(() => {
      this.tasksResource.reload()
    })
  }

}
