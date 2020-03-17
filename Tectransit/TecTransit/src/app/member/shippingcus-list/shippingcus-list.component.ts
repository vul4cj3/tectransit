import { Component, OnInit } from '@angular/core';
import { MemStationInfo, ShippingMCusInfo } from 'src/app/_Helper/models';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ShippstatusPipe } from 'src/app/_Helper/shippstatus.pipe';
import { FormGroup, FormBuilder } from '@angular/forms';

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
  private delUrl = 'DelShippingCusData';

  tableTitle = ['#', '集運單號', '主單號', '狀態', '建單時間', '發貨時間', '細項'];

  data: ShippingMCusInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;

  shippingType;
  tempList: any = [];
  chkList: any = [];
  srhList: any = [];

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService,
    private route: ActivatedRoute,
    private router: Router,
    public shippstatus: ShippstatusPipe
  ) { }

  ngOnInit() {
    this.shippingType = this.route.snapshot.paramMap.get('type');

    this.srhList.push({ status: this.shippingType });
    this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);

  }

  srhData() {
    this.srhList = [];
    const smawb = document.getElementById('smawbno') as HTMLInputElement;
    const sdate = document.getElementById('credates') as HTMLInputElement;
    const edate = document.getElementById('credatee') as HTMLInputElement;

    this.srhList.push({
      status: this.shippingType, mawbno: smawb.value, cresdate: sdate.value, creedate: edate.value
    });

    this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);
  }

  crePagination(newPage: number, pageurl) {
    this.commonService.getListData(this.srhList, newPage, this.pageSize, pageurl)
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
      const pageurl = this.baseUrl + this.transferdataUrl;

      this.commonService.getListData(this.srhList, newPage, this.pageSize, pageurl)
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
              this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);
              this.chkList = [];
            }
          },
            error => {
              console.log(error);
            });

      } else { alert('無項目被刪除！'); }
    } else { }
  }

}
