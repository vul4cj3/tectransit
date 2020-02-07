import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, throwIfEmpty } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  /* Common Variables*/
  private baseUrl = window.location.origin + '/api/CommonHelp/';

  /* Web api action url*/
  private navUrl = 'GetNavMenu_Front';
  private imgUploadUrl = 'UploadImgData_F';
  private fileUploadUrl = 'UploadFileData_F';
  private chkMemtypeUrl = 'GetMemtype';
  private getStationUrl = 'GetFirstStation';

  /* pagination variables*/
  rowTotal = 0; // data count

  currentPage = 1;
  pageSize = 10;
  pageTotal = 0;

  public pageNum: Array<number> = []; // pagenum in page
  pageNumStart = 0; // min page
  pageNumEnd = 0; // max page
  currentPageGroup = 0; // current group
  totalPageGroup = 0; // all group

  public preApper = false;
  public nextApper = false;
  public preGroupApper = false;
  public nextGroupApper = false;

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
    this.currentPage = scurrentPage;

    if (this.currentPage > this.pageTotal) {
      this.currentPage = this.pageTotal;
      scurrentPage = this.pageTotal;
    }
    if (this.currentPage < 0) {
      this.currentPage = 1;
      scurrentPage = 1;
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
    this.pageNumStart = scurrentPage % 10 === 0 ? (Math.floor(scurrentPage / 10) * 10) : (Math.floor(scurrentPage / 10) * 10 + 1);
    this.pageNumEnd = this.pageNumStart + (10 - 1);

    if (this.pageNumStart > this.pageTotal) {
      this.pageNumStart = this.pageTotal;
    }

    if (this.pageNumEnd > this.pageTotal) {
      this.pageNumEnd = this.pageTotal;
    }

    for (let i = this.pageNumStart; i <= this.pageNumEnd; i++) {
      this.pageNum.push(i);
    }

    // 防呆
    if (this.pageNum.length === 1 && this.pageNum[0] === 1) {
      this.pageNum = [];
    } else if (this.pageNum[0] === 0) {
      this.pageNum = [];
    }

    // Appear next or next group button
    this.nextGroupApper = (this.currentPageGroup < this.totalPageGroup) ? true : false;
    this.nextApper = (this.currentPage < this.pageTotal) ? true : false;

  }
  /* --- Pagination function end --- */

  /* --- CRUD Common function --- */
  getListData(sWhere, pageIndex: number, pageSize: number, pageUrl: string) {
    const postData = { srhForm: sWhere, PAGE_INDEX: pageIndex, PAGE_SIZE: pageSize };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  getAllData(sWhere, pageUrl: string) {
    const postData = { srhForm: sWhere };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  getData(id, pageUrl: string) {
    return this.http.get<any>(`${pageUrl}/${id}`)
      .pipe(map(data => {
        return data;
      }));
  }

  getData2(idlist, pageUrl: string) {
    const postData = { id: idlist };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  getSingleData(pageUrl: string) {
    return this.http.get<any>(`${pageUrl}`)
      .pipe(map(data => {
        return data;
      }));
  }

  insertData(form, pageUrl: string) {
    const postData = { formdata: form };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  editData(form, pageUrl: string) {
    const postData = { formdata: form };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  editSingleData(code, form, pageUrl: string) {
    const postData = { id: code, formdata: form };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  delData(arraydata, pageUrl: string) {
    const postData = { formdata: arraydata };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  delSingleData(id, pageUrl: string) {
    return this.http.get<any>(`${pageUrl}/${id}`)
      .pipe(map(data => {
        return data;
      }));
  }

  imageUpload(form) {
    return this.http.post<any>(this.baseUrl + this.imgUploadUrl, form)
      .pipe(map(data => {
        return data;
      }));
  }

  fileUpload(form) {
    return this.http.post<any>(this.baseUrl + this.fileUploadUrl, form)
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

  /* other function */
  chkMemtype() {
    return this.http.get<any>(this.baseUrl + this.chkMemtypeUrl)
      .pipe(map(data => {
        return data;
      }));
  }

  getStation() {
    return this.http.get<any>(this.baseUrl + this.getStationUrl)
      .pipe(map(data => {
        return data;
      }));
  }

}
