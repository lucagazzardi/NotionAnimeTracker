import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MAL_AnimeModel } from "../model/MAL_AnimeModel";


@Injectable()
export class SearchByNameModalService {

  baseUrl: string = "https://localhost:44389/api/"
  mainController: string = "main/"

  constructor(private client: HttpClient) { }

  getShowListByName(searchTerm: string) {
    return this.client.get<MAL_AnimeModel[]>(this.baseUrl + this.mainController + "mal/search/name?searchTerm=" + searchTerm);
  }

  postAddToNotion(show: MAL_AnimeModel) {
    return this.client.post(this.baseUrl + this.mainController + "notion/add", show);
  }
}

