import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-version-number',
  templateUrl: './version-number.component.html',
  styleUrls: ['./version-number.component.scss']
})
export class VersionNumberComponent implements OnInit {

  version: string;
  buildNumber: string;

  constructor(
    private httpClient: HttpClient
  ) { }

  ngOnInit(): void {
    this.httpClient.get('assets/version.txt', {
      responseType: 'text'
    })
      .subscribe((data) => {
        this.version = data.toString().trim();
      });

    this.httpClient.get('assets/build_no.txt', {
      responseType: 'text'
    })
      .subscribe((data) => {
        this.buildNumber = data.toString().trim();
      });
  }

}
