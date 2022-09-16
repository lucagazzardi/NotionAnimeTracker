import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MAL_AnimeModel } from "../model/MAL_AnimeModel";


@Injectable()
export class AppService {

  baseUrl: string = "https://localhost:44389/api/"
  mainController: string = "main/"

  constructor(private client: HttpClient) { }
}

