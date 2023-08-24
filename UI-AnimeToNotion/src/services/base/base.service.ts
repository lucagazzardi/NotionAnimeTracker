import { HttpClient, HttpEvent, HttpEventType, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { BehaviorSubject, last, map, Observable, Subject } from "rxjs";

@Injectable()
export class BaseService {

  baseUrl: string = environment.apiKey;
  trackOptions: object = { reportProgress: true, observe: 'events' };
  totalCoefficient: number = 1;

  callProgress: Subject<number> = new Subject<number>();

  constructor(private client: HttpClient) { }  

  get(url: string): Observable<any>  {

    let req = new HttpRequest('GET', this.baseUrl + url, this.trackOptions);
    return this.sendRequest(req);
  }

  post(url: string, body: object | null): Observable<any> {
    let req = new HttpRequest('POST', this.baseUrl + url, body, this.trackOptions);
    return this.sendRequest(req);
  }

  put(url: string, body: object | null): Observable<any> {
    let req = new HttpRequest('PUT', this.baseUrl + url, body, this.trackOptions);    
    return this.sendRequest(req);
  }

  delete(url: string): Observable<any> {
    let req = new HttpRequest('DELETE', this.baseUrl + url, this.trackOptions);
    return this.sendRequest(req);
  }

  /// Sends request to server keeping only the last result (Response)
  sendRequest(req: HttpRequest<any>) {
    this.callProgress.next(10);
    return this.client.request(req).pipe(
      map(event => this.handleEvent(event)),
      last()
    )
  }

  /// Tracks progress of the API call for the progress bar. Returns result when fully loaded
  handleEvent(event: HttpEvent<any>) {    
    switch (event.type) {
      case HttpEventType.DownloadProgress:
        if (event.total) {
          this.callProgress.next(Math.round(100 * event.loaded / event.total));
        }
        else {
          let currentCoef = event.loaded * (1 + this.totalCoefficient);
          this.callProgress.next(Math.round(100 * event.loaded / currentCoef));
        }
        this.totalCoefficient /= 2;        
        return null;
      case HttpEventType.Response:
        this.callProgress.next(100);
        this.totalCoefficient = 1;
        return event.body;
      default:
        return null;
    }    
  }
  
}

