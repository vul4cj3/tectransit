import { Component, OnInit } from '@angular/core';
import { CommonService } from '../../services/common.service';
import { NewsInfo } from '../../_Helper/models';

@Component({
  selector: 'app-news-list',
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.css']
})
export class NewsListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = '/api/FrontHelp/';
  private dataUrl = 'GetNewsData';

  data: NewsInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;

  srhList: any = [];

  constructor(
    public commonservice: CommonService
  ) { }

  ngOnInit(): void {
    this.crePagination(this.currentpage, this.baseUrl + this.dataUrl);
  }

  crePagination(newPage: number, pageurl) {
    this.commonservice.getListData(this.srhList, newPage, this.pageSize, pageurl)
      .subscribe(
        data => {
          if (data.total > 0) {
            this.data = data.rows;
            this.rowTotal = data.total;
            this.currentpage = newPage;

            this.commonservice.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
          } else {
            this.data = null;
            this.rowTotal = 0;
            this.currentpage = 1;

            this.commonservice.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
          }
        },
        error => {
          console.log(error);
        });
  }

  changeData(newPage: number) {
    if (newPage !== this.currentpage) {
      const pageurl = this.baseUrl + this.dataUrl;

      this.commonservice.getListData(this.srhList, newPage, this.pageSize, pageurl)
        .subscribe(
          data => {
            if (data.total > 0) {
              this.data = data.rows;
              this.rowTotal = data.total;
              this.currentpage = newPage;

              this.commonservice.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
            } else {
              this.data = null;
              this.rowTotal = 0;
              this.currentpage = 1;

              this.commonservice.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
            }
          },
          error => {
            console.log(error);
          });
    }
  }

}
