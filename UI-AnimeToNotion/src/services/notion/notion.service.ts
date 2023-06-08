import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MAL_AnimeModel } from "../../model/MAL_AnimeModel";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";


@Injectable()
export class NotionService {

  baseUrl: string = environment.apiKey;
  mainController: string = "main/"

  //MOCK DATA
  url: string = 'http://localhost:3000/demolist';

  constructor(private client: HttpClient) { }

  add(show: MAL_AnimeModel) {
    return this.client.post(this.baseUrl + this.mainController + "notion/add", show);
  }

  remove(id: number) {
    return "Removed";
  }

  search(term: string): Observable<any> {
    return this.client.get(this.url + "?title=" + term);
  }
}

