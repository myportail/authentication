import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-build-number',
  templateUrl: './build-number.component.html',
  styleUrls: ['./build-number.component.scss']
})
export class BuildNumberComponent implements OnInit {

  build_number: string;

  constructor(
    private httpClient: HttpClient
  ) { }

  ngOnInit(): void {
    this.httpClient.get('assets/build_no.txt', {
      responseType: 'text'
    })
      .subscribe((data) => {
        this.build_number = data.toString();
      });
  }

}

