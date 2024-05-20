//ANGULAR OR EXTERNAL RELATED
import { ChangeDetectorRef, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToasterModule } from 'gazza-toaster';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatMenuModule } from '@angular/material/menu';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { NgxWebstorageModule } from 'ngx-webstorage';
import { NgxScrollPositionRestorationModule } from 'ngx-scroll-position-restoration';
import { MatTabsModule } from '@angular/material/tabs';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

//APP COMPONENTS
import { AppComponent } from './app.component';
import { SearchAnimeComponent } from '../components/search-anime/search-anime.component';
import { EditComponent } from '../components/edit/edit.component';
import { SelectComponent } from '../components/form-components/select/select.component';
import { CounterComponent } from '../components/form-components/counter/counter.component';
import { DatepickerComponent } from '../components/form-components/datepicker/datepicker.component';
import { TextInputComponent } from '../components/form-components/text-input/text-input.component';
import { LibraryComponent } from '../components/library/library.component';
import { SearchbarComponent } from '../components/form-components/searchbar/searchbar.component';
import { SliderComponent } from '../components/form-components/slider/slider.component';
import { SortComponent } from '../components/sort/sort.component';
import { CheckboxComponent } from '../components/form-components/checkbox/checkbox.component';
import { HistoryComponent } from '../components/history/history.component';
import { HistoryElementComponent } from '../components/history-element/history-element.component';
import { FooterComponent } from '../components/footer/footer.component';
import { EpisodesComponent } from '../components/episodes/episodes.component';
import { AutocompleteComponent } from '../components/form-components/autocomplete/autocomplete.component';
import { MatChipsModule } from '@angular/material/chips';

//APP SERVICES
import { ThemeService } from '../services/theme/theme.service';
import { SearchAnimeService } from '../components/search-anime/search-anime.service';
import { InternalService } from '../services/internal/internal.service';
import { StringManipulationService } from '../services/string-manipulation/string-manipulation.service';
import { EditService } from '../services/edit/edit.service';
import { MalService } from '../services/mal/mal.service';
import { HistoryService } from '../services/history/history.service';
import { BaseService } from '../services/base/base.service';

@NgModule({
  declarations: [
    AppComponent,
    SearchAnimeComponent,
    EditComponent,
    SelectComponent,
    CounterComponent,
    DatepickerComponent,
    TextInputComponent,
    LibraryComponent,
    SearchbarComponent,
    SliderComponent,
    SortComponent,
    CheckboxComponent,
    HistoryComponent,
    HistoryElementComponent,
    FooterComponent,
    EpisodesComponent,
    AutocompleteComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ToasterModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatMenuModule,
    NgxSliderModule,
    MatDialogModule,
    MatMomentDateModule,
    InfiniteScrollModule,
    MatTooltipModule,
    MatProgressBarModule,
    NgxWebstorageModule.forRoot({ prefix: '', separator: '' }),
    NgxScrollPositionRestorationModule.forRoot(),
    MatTabsModule,
    MatAutocompleteModule,
    MatChipsModule,
    PerfectScrollbarModule
  ],
  providers: [
    ThemeService,
    BaseService,
    SearchAnimeService,
    InternalService,
    MalService,
    StringManipulationService,
    EditService,
    HistoryService,
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
