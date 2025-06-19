import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ThemeServiceService } from '../theme-service.service';

@Component({
  selector: 'app-theme-toggle',
  imports: [MatIconModule,MatButtonModule],
  templateUrl: './theme-toggle.component.html',
  styleUrl: './theme-toggle.component.scss'
})
export class ThemeToggleComponent {
  private themeSevice = inject(ThemeServiceService)
  isDarkTheme = this.themeSevice.isDarkTheme;
  toggleTheme(){
    this.themeSevice.toggleTheme();
  }
}
