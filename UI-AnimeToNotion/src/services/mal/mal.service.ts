import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { IAnimeBase } from "../../model/IAnimeBase";
import { IAnimeFull } from "../../model/IAnimeFull";
import { BaseService } from "../base/base.service";


@Injectable()
export class MalService {

  baseUrl: string = environment.apiKey;
  malController: string = "mal/"

  //MOCK DATA
  url = 'http://localhost:3000/demolist';

  constructor(private client: HttpClient, private httpService: BaseService) { }

  // MOCK
  getAll(): Observable<any> {
    return this.httpService.get(this.url);
  }

  getCurrentSeason(): Observable<IAnimeBase[]> {
    return this.httpService.get(this.malController + "get/season/current");
  }

  getUpcomingSeason(): Observable<IAnimeBase[]> {
    return this.httpService.get(this.malController + "get/season/upcoming");
  } 

  getShowFullById(id: string): Observable<IAnimeFull> {
    return this.httpService.get(this.malController + "get/anime/" + id);

  }

  searchByName(searchTerm: string) {
    return this.httpService.get(this.malController + "search/anime/" + searchTerm);
  }
}

