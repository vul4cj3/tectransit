import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { UserLog } from 'src/app/_Helper/models';

@Component({
  selector: 'app-userlog',
  templateUrl: './userlog.component.html',
  styleUrls: ['./userlog.component.css']
})
export class UserlogComponent implements OnInit {
  /* Web api url*/
  private baseUrl = window.location.origin + '/api/SysHelp/';
  private dataUrl = 'GetTSUserLogListData';

  tableTitle = ['#', '用戶', '用戶名稱', '頁面', '頁面名稱', '操作內容', '紀錄時間'];
  data: UserLog[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 10;
  srhForm: FormGroup;

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
      sstartdate: '',
      senddate: ''
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

}
