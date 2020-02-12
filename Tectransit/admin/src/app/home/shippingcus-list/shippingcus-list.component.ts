import { Component, OnInit } from '@angular/core';
import { StationInfo, ShippingMCusInfo } from 'src/app/_Helper/models';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { ShippstatusPipe } from 'src/app/_Helper/shippstatus.pipe';

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
  private baseUrl = window.location.origin + '/api/UserHelp/';
  private dataUrl = 'GetTVShippingMListData';
  private statusUrl = 'EditTVShippingMStatus';
  private delUrl = 'DelTVShippingMData';
  private stationUrl = '/api/CommonHelp/GetStationData';

  tableTitle = ['#', '集運站', '集運單號', '追蹤號碼', '快遞單號', '廠商', '會員帳號', '建單時間',
    '更新時間', '狀態', '編輯'];
  data: ShippingMCusInfo[];
  stationData: StationInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 10;
  srhForm: FormGroup;

  tempList: any = [];
  chkList: any = [];

  status = 0;

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService,
    private shippstatus: ShippstatusPipe
  ) { }

  ngOnInit() {
    this.getStation();
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
      sstationcode: 'ALL',
      sshippingno: '',
      strackingno: '',
      strasferno: '',
      sacccode: '',
      sstatus: this.status
    });
  }

  getStation() {
    this.commonService.getData(this.stationUrl)
      .subscribe(data => {
        this.stationData = data.rows;
      },
        error => {
          console.log(error);
        });
  }

  crePagination(newPage: number) {
    this.commonService.getListData(this.srhForm.value, newPage, this.pageSize, this.baseUrl + this.dataUrl)
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
      this.commonService.getListData(this.srhForm.value, newPage, this.pageSize, this.baseUrl + this.dataUrl)
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

  tabSwitch(id) {
    if (id === 'tab1') {
      this.status = 0;
      this.srhForm.controls.sstatus.setValue('0');
    } else if (id === 'tab2') {
      this.status = 1;
      this.srhForm.controls.sstatus.setValue('1');
    } else if (id === 'tab3') {
      this.status = 2;
      this.srhForm.controls.sstatus.setValue('2');
    } else if (id === 'tab4') {
      this.status = 3;
      this.srhForm.controls.sstatus.setValue('3');
    } else if (id === 'tab5') {
      this.status = 4;
      this.srhForm.controls.sstatus.setValue('4');
    } else { }

    this.crePagination(this.currentpage);
  }

  chgStatus(type) {
    if (this.chkList.length > 0) {
      this.commonService.editStatusData(type, this.chkList, this.baseUrl + this.statusUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.crePagination(this.currentpage);
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
    const IsConfirm = confirm('確定要刪除(連同集運單下所有細項資料一併刪除)？');
    if (IsConfirm === true) {
      if (this.chkList.length > 0) {
        this.commonService.delData(this.chkList, this.baseUrl + this.delUrl)
          .subscribe(data => {
            if (data.status === '0') {
              alert(data.msg);
              this.crePagination(this.currentpage);
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

}
