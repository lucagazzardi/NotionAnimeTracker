import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthcallbackComponent } from '../components/authcallback/authcallback.component';
import { EditComponent } from '../components/edit/edit.component';
import { HistoryElementComponent } from '../components/history-element/history-element.component';
import { HistoryComponent } from '../components/history/history.component';
import { LibraryComponent } from '../components/library/library.component';
import { SearchAnimeComponent } from '../components/search-anime/search-anime.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full', data: { title: 'Library' } },
  { path: 'library', redirectTo: '/home', pathMatch: 'full', data: { title: 'Library' } },
  { path: 'home', component: LibraryComponent, data: { title: 'Library' } },
  { path: 'edit/:id/:title', component: EditComponent, data: { title: 'Edit' } },
  { path: 'browse', component: SearchAnimeComponent, data: { title: 'Browse' } },
  { path: 'history', component: HistoryComponent, data: { title: 'History' } },
  { path: 'history/year/:year', component: HistoryElementComponent, data: { title: 'Year' } },
  { path: 'auth/callback', component: AuthcallbackComponent, data: { title: 'Redirecting' } }

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'disabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
