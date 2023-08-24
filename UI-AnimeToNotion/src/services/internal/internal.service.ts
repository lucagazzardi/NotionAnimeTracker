import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { IAnimeBase } from "../../model/IAnimeBase";
import { IAnimeEdit } from "../../model/IAnimeEdit";
import { IAnimeFull } from "../../model/IAnimeFull";
import { IQuery } from "../../model/IQuery";
import { BaseService } from "../base/base.service";


@Injectable()
export class InternalService {

  baseUrl: string = environment.apiKey;
  internalController: string = "internal/"

  constructor(private client: HttpClient, private httpService: BaseService) { }

  addBase(show: IAnimeBase): Observable<any> {
    //return this.client.post(this.baseUrl + this.internalController + "add/base", show, { reportProgress: true });
    return this.httpService.post(this.internalController + "add/base", show);
  }

  addFull(show: IAnimeFull): Observable<any> {
    //return this.client.post(this.baseUrl + this.internalController + "add/full", show);
    return this.httpService.post(this.internalController + "add/full", show);
  }

  remove(id: string) {
    //return this.client.delete(this.baseUrl + this.internalController + "delete/" + id);
    return this.httpService.delete(this.internalController + "delete/" + id);
  }

  getAnimeFull(malId: number): Observable<any> {
    //return this.client.get(this.baseUrl + this.internalController + "get/full/" + malId);
    return this.httpService.get(this.internalController + "get/full/" + malId);
  }

  getAnimeForEdit(id: string): Observable<any> {
    //return this.client.get(this.baseUrl + this.internalController + "get/edit/" + id);
    return this.httpService.get(this.internalController + "get/edit/" + id);
  }

  editAnime(animeEdit: IAnimeEdit) {
    //return this.client.post(this.baseUrl + this.internalController + "edit", animeEdit);
    return this.httpService.post(this.internalController + "edit", animeEdit);
  }

  setFavorite(id: string, favorite: boolean): Observable<any> {
    //return this.client.put(this.baseUrl + this.internalController + "set/favorite/" + id + "/" + favorite, null);
    return this.httpService.put(this.internalController + "set/favorite/" + id + "/" + favorite, null);
  }

  setPlanToWatch(id: string, planttowatch: boolean): Observable<any> {
    //return this.client.put(this.baseUrl + this.internalController + "set/plantowatch/" + id + "/" + planttowatch, null, { reportProgress: true, observe: 'events' });
    return this.httpService.put(this.internalController + "set/plantowatch/" + id + "/" + planttowatch, null);
  }

  getAnimeRelations(id: number): Observable<any> {
    //return this.client.get(this.baseUrl + this.internalController + "get/relations/" + id);
    return this.httpService.get(this.internalController + "get/relations/" + id);
  }

  libraryQuery(query: IQuery): Observable<any> {
    //return this.client.post(this.baseUrl + this.internalController + "get/filtered", query);
    return this.httpService.post(this.internalController + "get/filtered", query);
  }

  getHistory(): Observable<any> {
    //return this.client.get(this.baseUrl + this.internalController + "get/history");
    return this.httpService.get(this.internalController + "get/history");
  }

  getHistoryYear(year: string, page: number): Observable<any> {
    //return this.client.get(this.baseUrl + this.internalController + "get/history/" + year + "/" + page);
    return this.httpService.get(this.internalController + "get/history/" + year + "/" + page);
  }

  getHistoryCounts(year: string): Observable<any> {
    //return this.client.get(this.baseUrl + this.internalController + "get/history/count/" + year);
    return this.httpService.get(this.internalController + "get/history/count/" + year);
  }
}

