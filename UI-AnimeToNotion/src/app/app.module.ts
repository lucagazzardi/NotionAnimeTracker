//ANGULAR OR EXTERNAL RELATED
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ToasterModule } from 'gazza-toaster';

//APP COMPONENTS
import { AppComponent } from './app.component';
import { SearchAnimeComponent } from '../components/search-anime/search-anime.component';
import { EditComponent } from '../components/edit/edit.component';

//APP SERVICES
import { ThemeService } from '../components/utility-components/theme/theme.service';
import { SearchAnimeService } from '../components/search-anime/search-anime.service';
import { NotionService } from '../components/utility-components/notion/notion.service';
import { StringManipulationService } from '../components/utility-components/string-manipulation/string-manipulation.service';
import { EditService } from '../components/edit/edit.service';
import { MalService } from '../components/utility-components/mal/mal.service';

@NgModule({
  declarations: [
    AppComponent,
    SearchAnimeComponent,
    EditComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ToasterModule,

  ],
  providers: [ThemeService, SearchAnimeService, NotionService, MalService, StringManipulationService, EditService],
  bootstrap: [AppComponent]
})
export class AppModule { }
