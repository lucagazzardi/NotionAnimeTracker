import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-search-anime',
  templateUrl: './search-anime.component.html',
  styleUrls: ['./search-anime.component.scss']
})
export class SearchAnimeComponent implements OnInit {

  searchById: boolean = true;

  constructor() { }

  ngOnInit(): void {
  }

  switchMode(value: boolean) {
    this.searchById = value;
  }
}
