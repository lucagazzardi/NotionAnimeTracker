//ANGULAR OR EXTERNAL RELATED
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToasterModule } from 'gazza-toaster';
import { MdbDropdownModule } from 'mdb-angular-ui-kit/dropdown';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

//APP COMPONENTS
import { AppComponent } from './app.component';
import { SearchAnimeComponent } from '../components/search-anime/search-anime.component';
import { EditComponent } from '../components/edit/edit.component';
import { SelectComponent } from '../components/form-components/select/select.component';
import { RankingComponent } from '../components/form-components/ranking/ranking.component';
import { DatepickerComponent } from '../components/form-components/datepicker/datepicker.component';
import { TextInputComponent } from '../components/form-components/text-input/text-input.component';

//APP SERVICES
import { ThemeService } from '../services/theme/theme.service';
import { SearchAnimeService } from '../components/search-anime/search-anime.service';
import { NotionService } from '../services/notion/notion.service';
import { StringManipulationService } from '../services/string-manipulation/string-manipulation.service';
import { EditService } from '../components/edit/edit.service';
import { MalService } from '../services/mal/mal.service';

@NgModule({
  declarations: [
    AppComponent,
    SearchAnimeComponent,
    EditComponent,
    SelectComponent,
    RankingComponent,
    DatepickerComponent,
    TextInputComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ToasterModule,
    MdbDropdownModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  providers: [ThemeService, SearchAnimeService, NotionService, MalService, StringManipulationService, EditService],
  bootstrap: [AppComponent]
})
export class AppModule { }
