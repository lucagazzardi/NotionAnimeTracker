import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { IAnimeBase } from "../../model/IAnimeBase";
import { IAnimeEdit } from "../../model/IAnimeEdit";
import { IAnimeFull } from "../../model/IAnimeFull";
import { IQuery } from "../../model/IQuery";
import { BaseService } from "../base/base.service";
import { AnimeEpisode } from "../../model/IAnimeEpisode";


@Injectable()
export class InternalService {

  baseUrl: string = environment.apiKey;
  internalController: string = "internal/"

  constructor(private client: HttpClient, private httpService: BaseService) { }

  addBase(show: IAnimeBase): Observable<any> {
    return this.httpService.post(this.internalController + "add/base", show);
  }

  addFull(show: IAnimeFull): Observable<any> {
    return this.httpService.post(this.internalController + "add/full", show);
  }

  remove(id: number) {
    return this.httpService.delete(this.internalController + "delete/" + id);
  }

  getAnimeFull(malId: number): Observable<any> {
    return this.httpService.get(this.internalController + "get/full/" + malId);
  }

  getAnimeForEdit(id: number): Observable<any> {
    return this.httpService.get(this.internalController + "get/edit/" + id);
  }

  editAnime(animeEdit: IAnimeEdit) {
    return this.httpService.post(this.internalController + "edit", animeEdit);
  }  

  setFavorite(id: number, favorite: boolean): Observable<any> {
    return this.httpService.put(this.internalController + "set/favorite/" + id + "/" + favorite, null);
  }

  setPlanToWatch(id: number, planttowatch: boolean): Observable<any> {
    return this.httpService.put(this.internalController + "set/plantowatch/" + id + "/" + planttowatch, null);
  }

  getAnimeRelations(id: number): Observable<any> {
    return this.httpService.get(this.internalController + "get/relations/" + id);
  }

  libraryQuery(query: IQuery): Observable<any> {
    return this.httpService.post(this.internalController + "get/filtered", query);
  }

  getHistory(): Observable<any> {
    return this.httpService.get(this.internalController + "get/history");
  }

  getHistoryYear(year: string, page: number): Observable<any> {
    return this.httpService.get(this.internalController + "get/history/" + year + "/" + page);
  }

  getHistoryCounts(year: string): Observable<any> {
    return this.httpService.get(this.internalController + "get/history/count/" + year);
  }

  getAnimeEpisodes(id: number, malId: number): Observable<any> {
    return this.httpService.get(this.internalController + 'anime/' + malId + '/episode/record/' + id, false);
  }

  addAnimeEpisode(animeEpisode: AnimeEpisode) {
    return this.httpService.post(this.internalController + 'add/episode', animeEpisode);
  }

  editAnimeEpisode(animeEpisode: AnimeEpisode) {
    return this.httpService.put(this.internalController + 'edit/episode', animeEpisode);
  }

  deleteAnimeEpisode(id: number) {
    return this.httpService.delete(this.internalController + 'delete/episode/' + id);
  }

  getGenres() {
    return this.httpService.get(this.internalController + 'genres', false);
  }
}

