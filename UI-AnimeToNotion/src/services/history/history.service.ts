import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable()
export class HistoryService {

  year: string | null = null;
  completed: number | null = null;
  favorite: number | null = null;

  constructor(private router: Router) { }

  setYear(year: string, completed: number, favorite: number) {
    this.year = year;
    this.completed = completed;
    this.favorite = favorite;
  }

  getYear() {
    return { year: this.year, completed: this.completed, favorite: this.favorite };
  }

  /// Open History Year Component
  route(year: string, completed: number, favorite: number) {

    this.setYear(year, completed, favorite);
    this.router.navigate(['history/year', year]);
  }

}
