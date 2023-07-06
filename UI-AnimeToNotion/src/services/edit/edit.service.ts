import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { IAnimeBase } from "../../model/IAnimeBase";
import { IAnimeFull } from "../../model/IAnimeFull";
import { StringManipulationService } from "../string-manipulation/string-manipulation.service";


@Injectable()
export class EditService {

  item: IAnimeBase | IAnimeFull | null = null;

  constructor(private stringManipulation: StringManipulationService, private router: Router) { }

  setItem(item: IAnimeBase) {
    this.item = item;
  }

  getItem() {
    return this.item;
  }

  /// Open Edit Component
  editItem(item: IAnimeBase) {
    let title: string = this.stringManipulation.normalize(item.nameEnglish);

    this.setItem(item);
    this.router.navigate(['edit', item.malId, title]);
  }

}
