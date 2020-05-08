import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(
    private httpClient: HttpClient
  ) { }

  login(username: string, password: string) {
    this.httpClient.get('api/values', {})
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
