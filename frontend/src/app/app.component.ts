import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeServiceService } from './modules/shared/theme-service.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Admin-Dashboard';
  private themeSevice = inject(ThemeServiceService);
}
