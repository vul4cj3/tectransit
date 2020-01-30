import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TransferHInfo, MemStationInfo, ShippingHInfo } from 'src/app/_Helper/models';
import { ShippstatusPipe } from 'src/app/_Helper/shippstatus.pipe';
import { IfStmt } from '@angular/compiler';

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

  tableTitle = ['#', '快遞單號', '狀態', '建單時間', '更新時間', '細項'];
  tableTitle2 = ['#', '集運單號', '狀態', '付款狀態', '建單時間', '付款時間', '發貨時間', '瀏覽'];

  data: TransferHInfo[];
  data2: ShippingHInfo[];
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
      this.cateID = 'S1';
    }

    this.getCateData();

    this.srhList.push({ status: this.shippingType, stationcode: this.cateID });

    // 未入庫,已入庫
    if (this.shippingType === 't1' || this.shippingType === 't2') {
      this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);
    }

    // 待出貨,已出貨,已完成
    if (this.shippingType === 'st1' || this.shippingType === 'st2' || this.shippingType === 'st3') {
      this.crePagination(this.currentpage, this.baseUrl + this.shippingdataUrl);
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

      let pageurl = '';
      if (this.shippingType === 't1' || this.shippingType === 't2') {
        pageurl = this.baseUrl + this.transferdataUrl;
      }

      // 待出貨,已出貨,已完成
      if (this.shippingType === 'st1' || this.shippingType === 'st2' || this.shippingType === 'st3') {
        pageurl = this.baseUrl + this.shippingdataUrl;
      }

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
    document.location.href = '/member/shipping/' + this.shippingType + '/' + val;
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

  doCombine() {
    let idlist = '';
    if (this.chkList.length > 0) {
      sessionStorage.removeItem('transid');
      for (const item of this.chkList) {
        if (item.isenable === true) {
          idlist += (idlist === '' ? '' : ';') + item.id;
        }
      }
      sessionStorage.setItem('transid', idlist);
      this.router.navigate(['/member/shipping/combine']);
    } else {
      alert('請至少選擇一筆！');
    }
  }

}
