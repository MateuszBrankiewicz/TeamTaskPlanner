import { Component } from '@angular/core';
import { ThemeToggleComponent } from '../../../shared/theme-toggle/theme-toggle.component';


@Component({
  selector: 'app-dashboard-page',
  imports: [ThemeToggleComponent],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.scss'
})
export class DashboardPageComponent {

}
