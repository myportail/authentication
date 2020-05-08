import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ApiHttpClientService} from "./api-http-client.service";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(
    private httpClient: ApiHttpClientService
  ) { }

  login(username: string, password: string) {
    let options = {};
    this.httpClient.get('/api/values', options)
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
