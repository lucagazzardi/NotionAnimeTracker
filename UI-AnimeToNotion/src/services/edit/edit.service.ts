import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { MAL_AnimeModel } from "../../model/MAL_AnimeModel";
import { StringManipulationService } from "../string-manipulation/string-manipulation.service";


@Injectable()
export class EditService {

  item: MAL_AnimeModel | null = null;

  constructor(private stringManipulation: StringManipulationService, private router: Router,) { }

  setItem(item: MAL_AnimeModel) {
    this.item = item;
  }

  getItem() {
    return this.item;
  }

  /// Open Edit Component
  editItem(item: MAL_AnimeModel) {
    let title: string = this.stringManipulation.normalize(item.alternative_titles.en ? item.alternative_titles.en : item.title);

    this.setItem(item);
    this.router.navigate(['edit', item.id, title]);
  }

}
