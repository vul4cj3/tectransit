import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AngularWaitBarrier } from 'blocking-proxy/built/lib/angular_wait_barrier';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  /* Common Variables*/
  private baseUrl = window.location.origin + '/api/CommonHelp/';

  /* Web api action url*/
  private navUrl = 'GetNavMenu';

  /* pagination variables*/
  rowTotal = 0; // data count

  currentPage = 1;
  pageSize = 10;
  pageTotal = 0;

  pageNum: Array<number> = []; // pagenum in page
  pageNumStart = 0; // min page
  pageNumEnd = 0; // max page
  currentPageGroup = 0; // current group
  totalPageGroup = 0; // all group

  preApper = false;
  nextApper = false;
  preGroupApper = false;
  nextGroupApper = false;

  constructor(private http: HttpClient) {

  }

  /* --- Get sidebar data --- */
  getMenu(usercode) {
    const postData = { USERCODE: usercode };
    return this.http.post<any>(this.baseUrl + this.navUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  /* --- Pagination function start --- */
  set_pageNumArray(srowTotal, spageSize, scurrentPage) {
    // reset pagination array
    this.pageNum = [];

    this.rowTotal = srowTotal;
    this.pageSize = spageSize;

    // Total pages count
    this.pageTotal = Math.floor(this.rowTotal / this.pageSize);
    if ((this.rowTotal % this.pageSize) > 0) {
      this.pageTotal++;
    }

    // Current page count
    this.currentPage = scurrentPage + 1;

    if (this.currentPage > this.pageTotal) {
      this.currentPage = this.pageTotal;
      scurrentPage = this.pageTotal - 1;
    }
    if (this.currentPage < 0) {
      this.currentPage = 1;
      scurrentPage = 0;
    }

    // All groups count
    this.totalPageGroup = Math.floor(this.pageTotal / 10);
    if ((this.pageTotal % 10) > 0) {
      this.totalPageGroup++;
    }

    // Current group count
    this.currentPageGroup = (Math.floor(this.currentPage / 10)) + 1;

    // Apper previous or previous group button
    this.preGroupApper = (this.currentPageGroup > 1) ? true : false;
    this.preApper = (this.currentPage > 1) ? true : false;

    // Add page num buttons
    this.pageNumStart = (Math.floor(scurrentPage / 10) * 10 + 1);
    this.pageNumEnd = this.pageNumStart + (10 - 1);
    if (this.pageNumEnd > this.pageTotal) {
      this.pageNumEnd = this.pageTotal;
    }

    for (let i = this.pageNumStart; i <= this.pageNumEnd; i++) {
      this.pageNum.push(i);
    }

    // Appear next or next group button
    this.nextGroupApper = (this.currentPageGroup < this.totalPageGroup) ? true : false;
    this.nextApper = (this.currentPage < this.pageTotal) ? true : false;

  }
  /* --- Pagination function end --- */

  /* --- Page's CRUD Common function --- */
  getListData(pageIndex: number, pageSize: number, pageUrl: string) {
    const postData = { PAGE_INDEX: pageIndex, PAGE_SIZE: pageSize };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  getSingleData(id, pageUrl: string) {
    return this.http.get<any>(`${pageUrl}/${id}`)
      .pipe(map(data => {
        return data;
      }));
  }

  editSingleData(form, pageUrl: string) {
    const postData = { formdata: form };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  editEnableData(arraydata, pageUrl: string) {
    const postData = { formdata: arraydata };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  delData(arraydata, pageUrl: string) {
    const postData = { id: arraydata };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  /* form common function */
  formChanges(obj1, obj2) {
    const obj = {};
    for (const k in obj1) {
      if (obj1[k] !== obj2[k]) {
        obj[k] = obj1[k];
      }
    }
    return obj;
  }

}
