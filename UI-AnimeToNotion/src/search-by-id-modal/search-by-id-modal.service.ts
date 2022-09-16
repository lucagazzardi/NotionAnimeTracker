import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MAL_AnimeModel } from "../model/MAL_AnimeModel";


@Injectable()
export class SearchByIdModalService {

  baseUrl: string = "https://localhost:44389/api/"
  mainController: string = "main/"

  constructor(private client: HttpClient) { }

  getShowById(id: string) {
    return this.client.get<MAL_AnimeModel>(this.baseUrl + this.mainController + "mal/search/" + id);
  }
}

