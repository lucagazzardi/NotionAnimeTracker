import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MAL_AnimeModel } from "../model/MAL_AnimeModel";
import { environment } from "../environments/environment";


@Injectable()
export class SearchByIdModalService {

  baseUrl: string = environment.apiKey;
  mainController: string = "main/"

  constructor(private client: HttpClient) { }

  getShowById(id: string) {
    return this.client.get<MAL_AnimeModel>(this.baseUrl + this.mainController + "mal/search/" + id);
  }

  postAddToNotion(show: MAL_AnimeModel) {
    return this.client.post(this.baseUrl + this.mainController + "notion/add", show);
  }
}

