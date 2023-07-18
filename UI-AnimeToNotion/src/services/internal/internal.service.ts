import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { IAnimeBase } from "../../model/IAnimeBase";
import { IAnimeEdit } from "../../model/IAnimeEdit";
import { IAnimeFull } from "../../model/IAnimeFull";
import { IQuery } from "../../model/IQuery";


@Injectable()
export class InternalService {

  baseUrl: string = environment.apiKey;
  internalController: string = "internal/"

  constructor(private client: HttpClient) { }

  addBase(show: IAnimeBase): Observable<any> {
    return this.client.post(this.baseUrl + this.internalController + "add/base", show);
  }

  addFull(show: IAnimeFull): Observable<any> {
    return this.client.post(this.baseUrl + this.internalController + "add/full", show);
  }

  remove(id: string) {
    return this.client.delete(this.baseUrl + this.internalController + "delete/" + id);
  }

  getAnimeFull(malId: number): Observable<any> {
    return this.client.get(this.baseUrl + this.internalController + "get/full/" + malId);
  }

  getAnimeForEdit(id: string): Observable<any> {
    return this.client.get(this.baseUrl + this.internalController + "get/edit/" + id);
  }

  editAnime(animeEdit: IAnimeEdit) {
    return this.client.post(this.baseUrl + this.internalController + "edit", animeEdit);
  }

  setFavorite(id: string, favorite: boolean): Observable<any> {
    return this.client.put(this.baseUrl + this.internalController + "set/favorite/" + id + "/" + favorite, null);
  }

  setPlanToWatch(id: string, planttowatch: boolean): Observable<any> {
    return this.client.put(this.baseUrl + this.internalController + "set/plantowatch/" + id + "/" + planttowatch, null);
  }

  getAnimeRelations(id: number): Observable<any> {
    return this.client.get(this.baseUrl + this.internalController + "get/relations/" + id);
  }

  libraryQuery(query: IQuery): Observable<any> {
    return this.client.post(this.baseUrl + this.internalController + "get/filtered", query);
  }
}

