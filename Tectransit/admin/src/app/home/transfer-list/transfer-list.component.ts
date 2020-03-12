import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { StationInfo, TransferMInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-transfer-list',
  templateUrl: './transfer-list.component.html',
  styleUrls: ['./transfer-list.component.css']
})
export class TransferListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = window.location.origin + '/api/UserHelp/';
  private dataUrl = 'GetTETransferListData';
  private instoreUrl = 'EditTransferStatus_instore';
  private unstoreUrl = 'EditTransferStatus_unstore';
  private delUrl = 'DelTETransferData';
  private stationUrl = '/api/CommonHelp/GetStationData';

  tableTitle = ['#', '集運站代碼', '集運站', '快遞單號', '會員帳號', '會員', '建單時間',
    '更新時間', '狀態', '編輯'];
  data: TransferMInfo[];
  stationData: StationInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;
  srhForm: FormGroup;

  chkList: any = [];
  instoreList: any = [];
  chkNum = 1;

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService
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
      stransferno: '',
      sacccode: ''
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

  doinstore() {
    if (this.chkList.length > 0) {
      this.commonService.editEnableData(this.chkList, this.baseUrl + this.instoreUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.crePagination(this.currentpage);
            this.chkList = [];
            this.instoreList = [];
          } else {
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });

    } else { alert('頁面無項目變更！'); }
  }

  dostockout() {
    if (this.instoreList.length > 0) {
      this.commonService.editEnableData(this.instoreList, this.baseUrl + this.unstoreUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.crePagination(this.currentpage);
            this.chkList = [];
            this.instoreList = [];
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
              this.chkList = [];
              this.instoreList = [];
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

  instoreSelChange(val, Ischk) {
    this.chkNum = 0;
    this.instoreList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.instoreList.length === 0) {
      this.instoreList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.instoreList.push({ id: val, isenable: Ischk });
      }
    }

  }

}
