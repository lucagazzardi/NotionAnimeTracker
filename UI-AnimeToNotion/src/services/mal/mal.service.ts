import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { IAnimeBase } from "../../model/IAnimeBase";
import { IAnimeFull } from "../../model/IAnimeFull";


@Injectable()
export class MalService {

  baseUrl: string = environment.apiKey;
  malController: string = "mal/"

  //MOCK DATA
  url = 'http://localhost:3000/demolist';

  constructor(private client: HttpClient) { }

  getAll(): Observable<any> {
    return this.client.get(this.url);
  }

  getCurrentSeason(): Observable<IAnimeBase[]> {
    return this.client.get<IAnimeBase[]>(this.baseUrl + this.malController + "get/season/current");
  }

  getUpcomingSeason(): Observable<IAnimeBase[]> {
    return this.client.get<IAnimeBase[]>(this.baseUrl + this.malController + "get/season/upcoming");
  } 

  getShowFullById(id: string): Observable<IAnimeFull> {
    return this.client.get<IAnimeFull>(this.baseUrl + this.malController + "get/anime/" + id);
  }

  searchByName(searchTerm: string) {
    return this.client.get<IAnimeBase[]>(this.baseUrl + this.malController + "search/anime/" + searchTerm);
  }
}

