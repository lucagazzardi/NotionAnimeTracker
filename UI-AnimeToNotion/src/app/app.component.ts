import { Component } from '@angular/core';
import { ToasterService } from 'gazza-toaster';
import { ThemeService } from '../services/theme/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'UI-AnimeToNotion';

  constructor(private themeService: ThemeService, private toasterService: ToasterService) { }

  ngOnInit(): void {
    
  }

  switchTheme(darkTheme: boolean) {
    this.themeService.switchTheme(darkTheme);
  }

  isDarkTheme() {
    return this.themeService.isCurrentlyDarkTheme();
  }
}
