import { Component } from '@angular/core';
import { ThemeToggleComponent } from '../../../shared/theme-toggle/theme-toggle.component';
import { SharedModule } from "../../../shared/shared.module";
import { NavComponent } from "../../../shared/nav/nav.component";
import { MatCardModule } from '@angular/material/card';


@Component({
  imports: [NavComponent,MatCardModule],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.scss'
})
export class DashboardPageComponent {

}
