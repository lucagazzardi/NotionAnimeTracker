import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditComponent } from '../components/edit/edit.component';
import { HistoryElementComponent } from '../components/history-element/history-element.component';
import { HistoryComponent } from '../components/history/history.component';
import { LibraryComponent } from '../components/library/library.component';
import { SearchAnimeComponent } from '../components/search-anime/search-anime.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full', data: { title: 'home' } },
  { path: 'library', redirectTo: '/home', pathMatch: 'full', data: { title: 'home' } },
  { path: 'home', component: LibraryComponent, data: { title: 'home' } },
  { path: 'edit/:id/:title', component: EditComponent, data: { title: 'edit' } },
  { path: 'browse', component: SearchAnimeComponent, data: { title: 'browse' } },
  { path: 'history', component: HistoryComponent, data: { title: 'history' } },
  { path: 'history/year/:year', component: HistoryElementComponent, data: { title: 'history year' } }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
