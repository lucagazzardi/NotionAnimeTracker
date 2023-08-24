import { transition, trigger, useAnimation } from '@angular/animations';
import { ChangeDetectorRef, Component } from '@angular/core';
import { ToasterService } from 'gazza-toaster';
import { scaleUpOnEnter } from '../assets/animations/animations';
import { BaseService } from '../services/base/base.service';
import { ThemeService } from '../services/theme/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [
    trigger('scaleUpOnEnter', [
      transition(':enter', [
        useAnimation(scaleUpOnEnter)
      ])
    ]),
  ]
})
export class AppComponent {

  title = 'Anime Takusan';
  mobileMenuOpen: boolean = false;
  progressBarValue: number = 0;

  constructor(private themeService: ThemeService, private baseService: BaseService, private cd: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.baseService.callProgress.subscribe(progress => { this.progressBarValue = progress; this.cd.detectChanges(); });
  }

  switchTheme(darkTheme: boolean) {
    this.themeService.switchTheme(darkTheme);
  }

  isDarkTheme() {
    return this.themeService.isCurrentlyDarkTheme();
  }

  openCloseMobileMenu() {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  resetProgressBar() {
    if (this.progressBarValue == 100)
      this.progressBarValue = 0;
  }

  ngOnDestroy() {
    this.baseService.callProgress.unsubscribe();
  }
}
