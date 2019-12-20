import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  /* Common Variables*/
  private baseUrl = window.location.origin + '/api/CommonHelp/';

  /* Web api action url*/
  private navUrl = 'GetNavMenu';

  /* pagination variables*/
  rowTotal = 0; // 資料總筆數

  currentPage = 1;
  pageSize = 10; // 一頁顯示筆數
  pageTotal = 0;

  pageNum: Array<number> = []; // 頁面上顯示頁數
  pageNumStart = 0; // 最大頁數值
  pageNumEnd = 0; // 最大頁數值
  currentPageGroup = 0; // 目前所在群組
  totalPageGroup = 0; // 所有群組數

  preApper = false;
  nextApper = false;
  preGroupApper = false;
  nextGroupApper = false;

  constructor(private http: HttpClient) {

  }

  getMenu(usercode) {
    const postData = { USERCODE: usercode };
    return this.http.post<any>(this.baseUrl + this.navUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

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

}
