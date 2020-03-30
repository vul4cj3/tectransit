import { Component, OnInit } from '@angular/core';
import { StationInfo, ShippingMCusInfo, AccountInfo } from 'src/app/_Helper/models';
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
  private brokerdataUrl = 'GetBrokerData';
  private brokereditUrl = 'EditTVShippingMBroker';
  private statusUrl = 'EditTVShippingMStatus';
  private delUrl = 'DelTVShippingMData';
  private importUrl = 'CoverCusShippingData';

  tableTitle = ['#', '集運單號', '主單號', '廠商', '出口報關', '進口報關', '建單時間', '更新時間', '已消倉表', '材積與實重表', 'MAWB', '編輯'];
  data: ShippingMCusInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;
  srhForm: FormGroup;

  imBroker: AccountInfo[];
  exBroker: AccountInfo[];

  tempList: any = [];
  chkList: any = [];
  fileList: any = [];

  status = 0;

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService,
    private shippstatus: ShippstatusPipe
  ) { }

  ngOnInit() {
    this.resetData();
    this.getData();
  }

  searchData() {
    this.currentpage = 1;
    this.crePagination(this.currentpage);
  }

  resetData() {
    if (sessionStorage.getItem('cusstatus') === null) {
      sessionStorage.removeItem('cusstatus');
      sessionStorage.setItem('cusstatus', this.status.toString());
    } else { this.status = parseInt(sessionStorage.getItem('cusstatus'), 0); }

    // built form controls and default form value
    this.srhForm = this.formBuilder.group({
      sshippingno: '',
      smawbno: '',
      scompany: '',
      sacccode: '',
      sstatus: parseInt(sessionStorage.getItem('cusstatus'), 0)
    });

  }

  getData() {
    this.commonService.getData(this.baseUrl + this.brokerdataUrl)
      .subscribe(res => {
        if (res.status === '0') {
          this.imBroker = res.imList;
          this.exBroker = res.exList;
        }
        this.crePagination(this.currentpage);
      }, error => {
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

    sessionStorage.setItem('cusstatus', this.status.toString());

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

  fileChange(e) {
    this.fileList = [];
    if (e.target.files[0] !== undefined) {
      this.fileList.push({ file: e.target.files[0] });
    }
  }

  doImport() {
    const schk1 = document.getElementById('chkfile1') as HTMLInputElement;
    const schk2 = document.getElementById('chkfile2') as HTMLInputElement;
    const schk3 = document.getElementById('chkfile3') as HTMLInputElement;

    if (!schk1.checked && !schk2.checked && !schk3.checked) {
      return alert('必須選擇文件類別！');
    } else if (this.chkList.length !== 1) {
      return alert('請選擇一筆資料！');
    } else if (this.fileList.length !== 1) {
      return alert('請選擇要匯入的檔案！');
    }

    let type = '';
    if (schk1.checked) {
      type = 'SHIPPINGFILE';
    } else if (schk2.checked) {
      type = 'BROKERFILE';
    } else {
      type = 'MAWBFILE';
    }

    const formdata = new FormData();
    formdata.append('type', type);
    formdata.append('id', this.chkList[0]);
    formdata.append('fileUpload', this.fileList[0].file);

    this.commonService.Upload(formdata, this.baseUrl + this.importUrl)
      .subscribe(data => {
        if (data.status === '0') {
          alert(data.msg);
          this.crePagination(this.currentpage);
          this.chkList = [];
          this.fileList = [];
        } else {
          alert(data.msg);
        }
      },
        error => {
          console.log(error);
        });

  }

  doSeprate() {
    const selim = document.getElementById('selimbr') as HTMLSelectElement;
    const selex = document.getElementById('selexbr') as HTMLSelectElement;

    if (selim.value === '0' && selex.value === '0') {
      return alert('出口或進口報關行必須選擇！');
    } else if (this.chkList.length <= 0) {
      return alert('請至少選擇一筆資料！');
    }

    if (this.chkList.length > 0) {
      this.commonService.editBrokerData(selim.value, selex.value, this.chkList, this.baseUrl + this.brokereditUrl)
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
