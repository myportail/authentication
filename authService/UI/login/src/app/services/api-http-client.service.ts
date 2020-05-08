import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpHandler} from "@angular/common/http";
import {APP_BASE_HREF} from "@angular/common";

@Injectable({
  providedIn: 'root'
})
export class ApiHttpClientService extends HttpClient {

  constructor(@Inject(APP_BASE_HREF) private baseHref:string, handler: HttpHandler) {
    super(handler);
  }
}
