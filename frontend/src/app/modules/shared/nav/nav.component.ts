import { Component, inject, signal } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { ThemeToggleComponent } from "../theme-toggle/theme-toggle.component";
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../shared/services/auth.service';

@Component({
  selector: 'app-nav',
  imports: [MatSidenavModule, ThemeToggleComponent, RouterModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss'
})
export class NavComponent {
  matDrawer = signal(false);
  authService = inject(AuthService);
  changeButton(){
    this.matDrawer.set(!this.matDrawer());
  }
}
