import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditComponent } from '../components/edit/edit.component';
import { SearchAnimeComponent } from '../components/search-anime/search-anime.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full', data: { title: 'home' } },
  { path: 'home', component: SearchAnimeComponent, data: { title: 'home' } },
  { path: 'edit/:id/:title', component: EditComponent, data: { title: 'edit' } },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
