import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ApiHttpClientService} from "./api-http-client.service";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(
    private httpClientService: ApiHttpClientService,
    private httpClient: HttpClient
  ) { }

  login(username: string, password: string) {
    this.httpClientService.get('/api/values', {})
      .subscribe(
        (data) => {
        console.log(data);
        },
        (error) => {
        console.log(`failure : ${JSON.stringify(error)}`)
        });
  }

  get loggedIn(): boolean {
    return false;
  }
}
