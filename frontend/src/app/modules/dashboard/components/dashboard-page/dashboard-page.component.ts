import { Component, effect, inject, signal } from '@angular/core';
import { ThemeToggleComponent } from '../../../shared/theme-toggle/theme-toggle.component';
import { SharedModule } from "../../../shared/shared.module";
import { NavComponent } from "../../../shared/nav/nav.component";
import { MatCardModule } from '@angular/material/card';
import { DashboardService } from '../../dashboard.service';


@Component({
  imports: [NavComponent,MatCardModule],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.scss'
})

export class DashboardPageComponent {
  tasks = signal({})
  dashboardService = inject(DashboardService);
  constructor(){
    effect(() => {
      this.dashboardService.getDashboard().subscribe((response) => {
        this.tasks.set(response)
        console.log(response);
      })
    })
  }
}
