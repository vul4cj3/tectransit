import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { AboutCate } from 'src/app/_Helper/models';

@Component({
  selector: 'app-about-categorylist',
  templateUrl: './about-categorylist.component.html',
  styleUrls: ['./about-categorylist.component.css']
})
export class AboutCategorylistComponent implements OnInit {

  /* Web api url*/
  private baseUrl = window.location.origin + '/api/WebSetHelp/';
  private dataUrl = 'GetTDAboutListData';
  private enableUrl = 'EditTDAboutEnableData';
  private topUrl = 'EditTDAboutTopData';
  private delUrl = 'DelAboutData';

  tableTitle = ['#', '標題', '敘述', '建立時間', '建立者',
    '更新時間', '更新者', '停用', '編輯', '細項'];
  data: AboutCate[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 10;
  srhForm: FormGroup;

  chkList: any = [];
  activeList: any = [];
  topList: any = [];
  pUserid: string;
  pUsercode: string;
  chkNum = 1;

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService
  ) { }

  ngOnInit() {
    this.resetData();
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

  crePagination(newPage: number) {
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
