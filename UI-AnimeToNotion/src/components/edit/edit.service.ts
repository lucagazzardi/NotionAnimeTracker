import { Injectable } from "@angular/core";
import { MAL_AnimeModel } from "../../model/MAL_AnimeModel";


@Injectable()
export class EditService {

  item: MAL_AnimeModel | null = null;

  constructor() { }

  setItem(item: MAL_AnimeModel) {
    this.item = item;
  }

  getItem() {
    return this.item;
  }

}
