import { Component, OnInit } from '@angular/core';
import { CommonService } from '../../services/common.service';
import { AboutInfo, AboutCate } from '../../_Helper/models';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-custom-list',
  templateUrl: './custom-list.component.html',
  styleUrls: ['./custom-list.component.css']
})
export class CustomListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = '/api/FrontHelp/';
  private cateUrl = 'GetAboutCate';
  private catedataUrl = 'GetAboutCateData';
  private dataUrl = 'GetAboutListData';

  id;
  catedata: AboutCate;
  data: AboutInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;

  srhList: any = [];

  constructor(
    private route: ActivatedRoute,
    public commonservice: CommonService
  ) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getData(this.id);
  }

  getData(cateid) {
    if (cateid === '0') {
      // 取得預設第一筆分類
      const promise = new Promise((resolve, reject) => {
        this.commonservice.getSingleData(this.baseUrl + this.cateUrl)
          .subscribe(
            data => {
              this.id = data.rows[0].cateid;
              resolve('success');
            },
            error => {
              console.log(error);
            });
      });

      Promise.all([promise])
        .then(() => {
          this.srhList.push({ abouthid: this.id });

          this.getCateData(this.id);
          this.crePagination(this.currentpage, this.baseUrl + this.dataUrl);
        });
    } else {
      this.srhList.push({ abouthid: this.id });

      this.getCateData(this.id);
      this.crePagination(this.currentpage, this.baseUrl + this.dataUrl);
    }

  }

  getCateData(id) {
    this.commonservice.getData(id, this.baseUrl + this.catedataUrl)
      .subscribe(data => {
        this.catedata = data.rows;
      }, error => {
        console.log(error);
      });
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
