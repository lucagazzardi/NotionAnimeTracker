import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { MAL_AnimeModel } from "../../model/MAL_AnimeModel";


@Injectable()
export class SearchAnimeService {
  searchTerm: string = "";

  url = 'http://localhost:3000/demolist';

  baseUrl: string = environment.apiKey;
  mainController: string = "main/"

  constructor(private client: HttpClient) { }

  getAll(): Observable<any> {
    return this.client.get(this.url);
  }

  getShowById(id: string) {
    return this.client.get<MAL_AnimeModel>(this.baseUrl + this.mainController + "mal/search/" + id);
  }

  getShowListByName(searchTerm: string) {
    return this.client.get<MAL_AnimeModel[]>(this.baseUrl + this.mainController + "mal/search/name?searchTerm=" + searchTerm);
  }
}

