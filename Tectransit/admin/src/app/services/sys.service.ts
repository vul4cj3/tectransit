import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SysService {
  /* Common Variables*/
  private baseUrl = window.location.origin + '/api/SysHelp/';

  /* Web api action url*/
  private roleUrl = 'GetTSRoleData';

  constructor(private http: HttpClient) { }

  getRoleLitData(pageIndex: number, pageSize: number) {
    const postData = { PAGE_INDEX: pageIndex, PAGE_SIZE: pageSize };
    return this.http.post<any>(this.baseUrl + this.roleUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

}
