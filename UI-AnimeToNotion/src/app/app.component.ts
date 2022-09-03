import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'UI-AnimeToNotion';

  onSearchName(value : string) {
    alert(value);
  }

  onSearchId(id: string) {
    alert(id);
  }
}
