import { Component, signal } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { ThemeToggleComponent } from "../theme-toggle/theme-toggle.component";
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-nav',
  imports: [MatSidenavModule, ThemeToggleComponent, RouterModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss'
})
export class NavComponent {
  matDrawer = signal(false);
  changeButton(){
    this.matDrawer.set(!this.matDrawer());
  }
}
