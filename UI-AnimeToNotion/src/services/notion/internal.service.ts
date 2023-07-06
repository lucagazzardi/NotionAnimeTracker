import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { observable, Observable, of } from "rxjs";
import { IAnimeBase } from "../../model/IAnimeBase";
import { IAnimePersonal } from "../../model/IAnimePersonal";
import { IAnimeEdit } from "../../model/IAnimeEdit";
import { IAnimeFull } from "../../model/IAnimeFull";


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

  search(dio: string): Observable<any> {
    return of(dio);
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

  getAnimeRelations(id: number): Observable<any> {
    return this.client.get(this.baseUrl + this.internalController + "get/relations/" + id);
  }
}

