import { Component, OnInit } from '@angular/core';
import { MemStationInfo, ShippingMCusInfo } from 'src/app/_Helper/models';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ShippstatusPipe } from 'src/app/_Helper/shippstatus.pipe';
import { resolve } from 'url';

@Component({
  selector: 'app-shippingcus-list',
  templateUrl: './shippingcus-list.component.html',
  styleUrls: ['./shippingcus-list.component.css'],
  providers: [
    ShippstatusPipe
  ]
})
export class ShippingcusListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = '/api/Member/';
  private transferdataUrl = 'GetShippingCusData';
  private catedataUrl = 'GetStationData';
  private delUrl = 'DelShippingCusData';

  tableTitle = ['#', '集運單號', '追蹤號碼', '狀態', '建單時間', '發貨時間', '細項'];

  data: ShippingMCusInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;

  cateData: MemStationInfo;
  cateID;
  shippingType;
  chkList: any = [];
  srhList: any = [];
  chkNum = 1;

  constructor(
    public commonService: CommonService,
    private route: ActivatedRoute,
    private router: Router,
    public shippstatus: ShippstatusPipe
  ) { }

  ngOnInit() {
    this.shippingType = this.route.snapshot.paramMap.get('type');
    this.cateID = this.route.snapshot.paramMap.get('id');

    if (this.cateID === '0') {
      // 取得預設第一筆站別
      const promise = new Promise((resolve, reject) => {
        this.commonService.getSingleData(this.baseUrl + this.catedataUrl)
          .subscribe(
            data => {
              this.cateData = data.rows;
              resolve('success');
            },
            error => {
              console.log(error);
            });
      });

      Promise.all([promise])
      .then(() => {
        this.cateID = this.cateData[0].stationcode;

        this.srhList.push({ status: this.shippingType, stationcode: this.cateID });
        this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);
      });

    } else {
      this.getCateData();
      this.srhList.push({ status: this.shippingType, stationcode: this.cateID });
      this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);
    }
  }

  getCateData() {
    this.commonService.getSingleData(this.baseUrl + this.catedataUrl)
      .subscribe(
        data => {
          this.cateData = data.rows;
        },
        error => {
          console.log(error);
        });
  }

  crePagination(newPage: number, pageurl) {
    this.commonService.getListData(this.srhList, newPage, this.pageSize, pageurl)
      .subscribe(
        data => {
          if (data.total > 0) {
            for (const item of data.rows) {
              item.status = this.shippstatus.transform(item.status);
            }
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
      const pageurl = this.baseUrl + this.transferdataUrl;

      this.commonService.getListData(this.srhList, newPage, this.pageSize, pageurl)
        .subscribe(
          data => {
            if (data.total > 0) {
              for (const item of data.rows) {
                item.status = this.shippstatus.transform(item.status);
              }
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

  cateChange(val) {
    document.location.href = '/member/shippingcus/' + this.shippingType + '/' + val;
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

  doDelete() {
    const IsConfirm = confirm('確定要刪除？');
    if (IsConfirm === true) {
      if (this.chkList.length > 0) {
        this.commonService.delData(this.chkList, this.baseUrl + this.delUrl)
          .subscribe(data => {
            if (data.status === '0') {
              alert(data.msg);
              this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);
              this.chkList = [];
            } else {
              alert(data.msg);
            }
          },
            error => {
              console.log(error);
            });

      } else { alert('無項目被刪除！'); }
    } else { }
  }

}
