import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  /* Common Variables*/
  private baseUrl = window.location.origin + '/api/CommonHelp/';
  private sysUrl = window.location.origin + '/api/SysHelp/';

  /* Web api action url*/
  private navUrl = 'GetNavMenu';
  private allnavUrl = 'GetAllMenu';
  private allmenuUrl = 'GetAllBacknFrontMenu';
  private allroleUrl = 'GetAllRole';
  private allrankUrl = 'GetAllRank';
  private allbannerUrl = 'GetAllBanner';
  private resetPWUrl = 'ResetPassword';
  private pbacknforntUrl = 'GetParentMenu';

  /* ckeditor config settings*/
  public editorConfig = {
    toolbarGroups: [{ name: 'document', groups: ['mode'] },
    { name: 'clipboard', groups: ['undo'] },
    { name: 'editing', groups: ['find', 'selection'] },
    { name: 'styles' },
      '/',
    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
    { name: 'colors', groups: ['TextColor', 'BGColor'] },
    { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },
    { name: 'links' },
    { name: 'insert' },
      '/'
    ],
    removeButtons: 'Strike,Subscript,Superscript,Anchor,Styles,Specialchar',
    filebrowserImageUploadUrl: '/test2/',
    height: 300
  };

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

  /* --- Get Data --- */
  getBacknFrontMenu() {
    return this.http.get<any>(`${this.baseUrl + this.allmenuUrl}/`)
      .pipe(map(data => {
        return data;
      }));
  }

  getParentMenu(isBack) {
    return this.http.get<any>(`${this.baseUrl + this.pbacknforntUrl}/${isBack}`)
      .pipe(map(data => {
        return data;
      }));
  }

  getDecnRecData(isDec, code, pageUrl) {
    return this.http.get<any>(`${pageUrl}/${isDec}/${code}`)
      .pipe(map(data => {
        return data;
      }));
  }

  /* --- System settings power data --- */
  getAllMenu(code) {
    return this.http.get<any>(`${this.baseUrl + this.allnavUrl}/${code}`)
      .pipe(map(data => {
        return data;
      }));
  }

  getAllRole(code) {
    return this.http.get<any>(`${this.baseUrl + this.allroleUrl}/${code}`)
      .pipe(map(data => {
        return data;
      }));
  }

  getAllRank(code) {
    return this.http.get<any>(`${this.baseUrl + this.allrankUrl}/${code}`)
      .pipe(map(data => {
        return data;
      }));
  }

  getAllBanner() {
    return this.http.get<any>(`${this.baseUrl + this.allbannerUrl}/`)
      .pipe(map(data => {
        return data;
      }));
  }

  /* System settings reset data */
  resetPW(id) {
    return this.http.get<any>(`${this.sysUrl + this.resetPWUrl}/${id}`)
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

  /* --- Page's CRUD Common function --- */
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

  getSingleData(id, pageUrl: string) {
    return this.http.get<any>(`${pageUrl}/${id}`)
      .pipe(map(data => {
        return data;
      }));
  }

  getInfoListData(cateID, sWhere, pageIndex: number, pageSize: number, pageUrl: string) {
    const postData = { CID: cateID, srhForm: sWhere, PAGE_INDEX: pageIndex, PAGE_SIZE: pageSize };
    return this.http.post<any>(pageUrl, postData)
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

  editPowerData(code: string, arraydata, pageUrl: string) {
    const postData = { id: code, formdata: arraydata };
    return this.http.post<any>(pageUrl, postData)
      .pipe(map(data => {
        return data;
      }));
  }

  editTopData(arraydata, pageUrl: string) {
    const postData = { formdata: arraydata };
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

  fileUpload(formData, pageUrl: string) {
    return this.http.post<any>(pageUrl, formData)
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
