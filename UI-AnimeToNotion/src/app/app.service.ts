import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MAL_AnimeModel } from "../model/MAL_AnimeModel";
import { Notion_LatestAddedModel } from "../model/Notion_LatestAddedModel";


@Injectable()
export class AppService {

  baseUrl: string = "https://localhost:44389/api/"
  mainController: string = "main/"

  constructor(private client: HttpClient) { }

  getLatestAdded() {
    return this.client.get<Notion_LatestAddedModel[]>(this.baseUrl + this.mainController + "notion/get/latestadded")
  }
}

