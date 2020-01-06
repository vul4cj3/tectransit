import { Component, OnInit } from '@angular/core';
import { AboutInfo, AboutCate } from 'src/app/_Helper/models';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-about-list',
  templateUrl: './about-list.component.html',
  styleUrls: ['./about-list.component.css']
})
export class AboutListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = window.location.origin + '/api/WebSetHelp/';
  private dataUrl = 'GetTDAboutListData';
  private catedataUrl = 'GetTDAboutCateData';
  private enableUrl = 'EditTDAboutEnableData';
  private topUrl = 'EditTDAboutTopData';
  private delUrl = 'DelAboutData';

  tableTitle = ['#', '標題', '建立時間', '建立者',
    '更新時間', '更新者', '置頂', '停用', '編輯'];
  data: AboutInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 10;
  srhForm: FormGroup;


  cateID: number;
  cateData: AboutCate[];
  chkList: any = [];
  activeList: any = [];
  topList: any = [];
  chkNum = 1;

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService,
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit() {
    // get parameters id then get data
    this.cateID = Number(this.route.snapshot.paramMap.get('id'));

    this.resetData();
    this.getCateData();
    this.crePagination(this.currentpage);
  }

  searchData() {
    this.currentpage = 1;
    this.crePagination(this.currentpage);
  }

  resetData() {
    // built form controls and default form value
    this.srhForm = this.formBuilder.group({
      skeyword: ''
    });
  }

  cateChange(val) {
    document.location.href = '/admin/about/info/' + val;
  }

  getCateData() {
    this.commonService.getAllData('', this.baseUrl + this.catedataUrl)
      .subscribe(
        data => {
          if (data.total > 0) {
            this.cateData = data.rows;

          } else {
            this.cateData = null;
          }
        },
        error => {
          console.log(error);
        });
  }


  crePagination(newPage: number) {
    this.commonService.getInfoListData(this.cateID, this.srhForm.value, newPage, this.pageSize, this.baseUrl + this.dataUrl)
      .subscribe(
        data => {
          if (data.total > 0) {
            this.data = data.rows;
            this.rowTotal = data.total;
            this.currentpage = newPage;

            this.commonService.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
          } else {
            this.data = null;
            this.rowTotal = 0;
            this.currentpage = 1;

            this.commonService.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
          }
        },
        error => {
          console.log(error);
        });
  }

  changeData(newPage: number) {
    if (newPage !== this.currentpage) {
      this.commonService.getListData(this.srhForm.value, newPage, this.pageSize, this.baseUrl + this.dataUrl)
        .subscribe(
          data => {
            if (data.total > 0) {
              this.data = data.rows;
              this.rowTotal = data.total;
              this.currentpage = newPage;

              this.commonService.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
            } else {
              this.data = null;
              this.rowTotal = 0;
              this.currentpage = 1;

              this.commonService.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
            }
          },
          error => {
            console.log(error);
          });
    }
  }

  doIstop() {
    if (this.topList.length > 0) {
      this.commonService.editTopData(this.topList, this.baseUrl + this.topUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.crePagination(this.currentpage);
            this.topList = [];
            this.activeList = [];
            this.chkList = [];
          } else {
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });

    } else { alert('頁面無項目變更！'); }
  }

  doIsenable() {
    if (this.activeList.length > 0) {
      this.commonService.editEnableData(this.activeList, this.baseUrl + this.enableUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.crePagination(this.currentpage);
            this.topList = [];
            this.activeList = [];
            this.chkList = [];
          } else {
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });

    } else { alert('頁面無項目變更！'); }
  }

  doDelete() {
    const IsConfirm = confirm('確定要刪除？');
    if (IsConfirm === true) {
      if (this.chkList.length > 0) {
        this.commonService.delData(this.chkList, this.baseUrl + this.delUrl)
          .subscribe(data => {
            if (data.status === '0') {
              alert(data.msg);
              this.crePagination(this.currentpage);
              this.topList = [];
              this.activeList = [];
              this.chkList = [];
            } else {
              alert(data.msg);
            }
          },
            error => {
              console.log(error);
            });

      } else { alert('頁面無項目變更！'); }
    } else { }
  }

  SelChange(val, Ischk) {
    this.chkNum = 0;
    this.chkList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.chkList.length === 0) {
      this.chkList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.chkList.push({ id: val, isenable: Ischk });
      }
    }

  }

  topSelChange(val, Ischk) {
    this.chkNum = 0;
    this.topList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.topList.length === 0) {
      this.topList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.topList.push({ id: val, isenable: Ischk });
      }
    }

  }

  enableSelChange(val, Ischk) {
    this.chkNum = 0;
    this.activeList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.activeList.length === 0) {
      this.activeList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.activeList.push({ id: val, isenable: Ischk });
      }
    }

  }

}
