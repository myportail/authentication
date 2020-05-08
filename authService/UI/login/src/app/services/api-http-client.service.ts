import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpEvent, HttpHandler, HttpHeaders, HttpParams} from "@angular/common/http";
import {APP_BASE_HREF} from "@angular/common";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiHttpClientService extends HttpClient {

  constructor(@Inject(APP_BASE_HREF) private baseHref:string, handler: HttpHandler) {
    super(handler);
  }

  get(url: string, options: any): Observable<any> {
    let urlParseResult = /^(\/)*(.*)$/.exec(url);
    let newUrl = `${this.baseHref}${urlParseResult[2]}`;
    return super.get(newUrl, options);
  }
}
