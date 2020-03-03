import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TransferMInfo, MemStationInfo, ShippingMInfo } from 'src/app/_Helper/models';
import { ShippstatusPipe } from 'src/app/_Helper/shippstatus.pipe';

@Component({
  selector: 'app-shipping-list',
  templateUrl: './shipping-list.component.html',
  styleUrls: ['./shipping-list.component.css'],
  providers: [
    ShippstatusPipe
  ]
})
export class ShippingListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = '/api/Member/';
  private transferdataUrl = 'GetACTransferData';
  private shippingdataUrl = 'GetACShippingData';
  private catedataUrl = 'GetStationData';
  private delUrl = 'DelACTransferData';

  tableTitle = ['#', '快遞單號', '訂單狀態', '建單時間', '更新時間', '細項'];
  tableTitle2 = ['#', '集運單號', '追蹤號碼', '訂單狀態', '付款狀態', '付款時間', '建單時間', '發貨時間', '細項'];

  data: TransferMInfo[];
  data2: ShippingMInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;

  cateData: MemStationInfo;
  cateID;
  shippingType;
  tempList: any = [];
  chkList: any = [];
  srhList: any = [];

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

          let page = this.baseUrl + this.shippingdataUrl;
          if (this.shippingType === 't1' || this.shippingType === 't2') {
            page = this.baseUrl + this.transferdataUrl;
          }
          this.crePagination(this.currentpage, page);
        });

    } else {
      this.getCateData();
      this.srhList.push({ status: this.shippingType, stationcode: this.cateID });

      let page = this.baseUrl + this.shippingdataUrl;
      if (this.shippingType === 't1' || this.shippingType === 't2') {
        page = this.baseUrl + this.transferdataUrl;
      }
      this.crePagination(this.currentpage, page);
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
            if (this.shippingType === 't1' || this.shippingType === 't2') {
              this.data = data.rows;
            } else {
              this.data2 = data.rows;
            }

            this.rowTotal = data.total;
            this.currentpage = newPage;

            this.commonService.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
          } else {
            this.data = null;
            this.data2 = null;
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
      let pageurl = this.baseUrl + this.shippingdataUrl;
      if (this.shippingType === 't1' || this.shippingType === 't2') {
        pageurl = this.baseUrl + this.transferdataUrl;
      }

      this.commonService.getListData(this.srhList, newPage, this.pageSize, pageurl)
        .subscribe(
          data => {
            if (data.total > 0) {
              if (this.shippingType === 't1' || this.shippingType === 't2') {
                this.data = data.rows;
              } else {
                this.data2 = data.rows;
              }

              this.rowTotal = data.total;
              this.currentpage = newPage;

              this.commonService.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
            } else {
              this.data = null;
              this.data2 = null;
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
    document.location.href = '/member/shipping/' + this.shippingType + '/' + val;
  }

  SelChange(val, Ischk) {
    this.tempList = [];
    this.chkList.map((item) => {
      if (item !== val) {
        this.tempList.push(item);
      }
    });

    this.chkList = this.tempList;

    if (Ischk) {
      this.chkList.push(val);
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
