import { transition, trigger, useAnimation } from '@angular/animations';
import { ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationStart, NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';
import { scaleUpOnEnter } from '../assets/animations/animations';
import { BaseService } from '../services/base/base.service';
import { ThemeService } from '../services/theme/theme.service';
import { LocalStorageService } from 'ngx-webstorage';
import { Location, PopStateEvent } from "@angular/common";


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
  siteTheme: string = '';

  private lastPoppedUrl: string | undefined;
  private yScrollStack: number[] = [];

  constructor(
    private themeService: ThemeService,
    private baseService: BaseService,
    private cd: ChangeDetectorRef,
    private router: Router,
    private titleService: Title,
    private route: ActivatedRoute,
    private localStorage: LocalStorageService,
    private location: Location
  ) { }

  ngOnInit(): void {

    // Page Title Handling
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        map(() => {
          const child: ActivatedRoute | null = this.route.firstChild;
          let title = child && child.snapshot.data['title'];
          if (title) {
            return title;
          }
        })
      )
      .subscribe((title) => {
        if (title) {
          this.titleService.setTitle(`Anime Takusan | ${title}`);
        }
      });

    // Progress Bar Handling
    this.baseService.callProgress.subscribe(progress => { this.progressBarValue = progress; this.cd.detectChanges(); });

    // Theme Cookie Handling
    this.siteTheme = this.localStorage.retrieve('site-theme');
    if (this.siteTheme == 'light')
      this.switchTheme(false);
  }

  ngAfterViewInit() {
    ////Scroll Handling
    //this.location.subscribe((ev: PopStateEvent) => {
    //  this.lastPoppedUrl = ev.url;
    //});
    //this.router.events.subscribe((ev: any) => {
    //  if (ev instanceof NavigationStart) {
    //    if (ev.url != this.lastPoppedUrl)
    //      this.yScrollStack.push(window.scrollY);
    //  } else if (ev instanceof NavigationEnd) {
    //    if (ev.url == this.lastPoppedUrl) {
    //      this.lastPoppedUrl = undefined;
    //      this.cd.detectChanges();
    //      window.scrollTo(0, this.yScrollStack.pop()!);
    //    } else
    //      window.scrollTo(0, 0);
    //  }
    //});
  }

  switchTheme(darkTheme: boolean) {    
    this.themeService.switchTheme(darkTheme);
    this.localStorage.store('site-theme', darkTheme ? 'dark' : 'light');
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
