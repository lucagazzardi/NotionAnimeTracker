import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { MAL_AnimeModel } from "../../model/MAL_AnimeModel";


@Injectable()
export class MalService {

  baseUrl: string = environment.apiKey;
  mainController: string = "main/"

  //MOCK DATA
  url = 'http://localhost:3000/demolist';

  constructor(private client: HttpClient) { }

  getAll(): Observable<any> {
    return this.client.get(this.url);
  }

  getShowById(id: string): Observable<MAL_AnimeModel> {
    return this.client.get<MAL_AnimeModel>(this.baseUrl + this.mainController + "mal/search/" + id);
  }

  getShowListByName(searchTerm: string) {
    return this.client.get<MAL_AnimeModel[]>(this.baseUrl + this.mainController + "mal/search/name?searchTerm=" + searchTerm);
  }
}

