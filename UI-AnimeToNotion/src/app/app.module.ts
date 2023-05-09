import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MdbCheckboxModule } from 'mdb-angular-ui-kit/checkbox';
import { MdbCollapseModule } from 'mdb-angular-ui-kit/collapse';
import { MdbRippleModule } from 'mdb-angular-ui-kit/ripple';
import { MdbModalModule } from 'mdb-angular-ui-kit/modal';
import { MdbTooltipModule } from 'mdb-angular-ui-kit/tooltip';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';


import { AppComponent } from './app.component';
import { SearchByIdModalComponent } from '../search-by-id-modal/search-by-id-modal.component';
import { SearchByIdModalModule } from '../search-by-id-modal/search-by-id-modal.module';
import { SearchByNameModule } from '../search-by-name-modal/search-by-name-modal.module';
import { AppService } from './app.service';
import { SearchAnimeComponent } from '../components/search-anime/search-anime.component';
import { ThemeService } from '../components/utility-components/theme/theme.service';
import { SearchAnimeService } from '../components/search-anime/search-anime.service';
import { FormsModule } from '@angular/forms';
import { NotionService } from '../components/utility-components/notion/notion.service';

@NgModule({
  declarations: [
    AppComponent,
    SearchAnimeComponent
  ],
  imports: [
    BrowserModule,
    MdbCheckboxModule,
    MdbCollapseModule,
    MdbRippleModule,
    MdbModalModule,
    MdbTooltipModule,
    SearchByIdModalModule,
    BrowserAnimationsModule,
    SearchByNameModule,
    HttpClientModule,
    NgxSkeletonLoaderModule,
    FormsModule
  ],
  providers: [AppService, ThemeService, SearchAnimeService, NotionService],
  bootstrap: [AppComponent],
  entryComponents: [SearchByIdModalComponent]
})
export class AppModule { }
