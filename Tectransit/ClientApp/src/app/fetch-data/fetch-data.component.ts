import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: sRole[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    debugger
    http.get<sRole[]>(baseUrl + 'api/TSRoles').subscribe(result => {
      this.forecasts = result;      
    }, error => console.error(error));
  }
}

interface sRole {
  rolecode: string;
  roleseq: number;
  rolename: string;
  isenable: boolean;
}
