import { Component, OnInit } from '@angular/core';
import { ShippingMCusInfo } from 'src/app/_Helper/models';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-shippingcus-list',
  templateUrl: './shippingcus-list.component.html',
  styleUrls: ['./shippingcus-list.component.css']
})
export class ShippingcusListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = '/api/Member/';
  private transferdataUrl = 'GetShippingCusData';
  private delUrl = 'DelShippingCusData';
  private importUrl = 'CoverCusShippingData';

  tableTitle = ['#', '集運單號', '狀態', '建單時間', '發貨時間', '材積與實重表', '細項'];

  data: ShippingMCusInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;

  shippingType;
  tempList: any = [];
  chkList: any = [];
  srhList: any = [];
  fileList: any = [];

  constructor(
    public commonservice: CommonService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.shippingType = this.route.snapshot.paramMap.get('type');

    this.srhList.push({ status: this.shippingType });
    this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);

  }

  srhData() {
    this.srhList = [];
    const sdate = document.getElementById('credates') as HTMLInputElement;
    const edate = document.getElementById('credatee') as HTMLInputElement;

    this.srhList.push({
      status: this.shippingType, cresdate: sdate.value, creedate: edate.value
    });

    this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);
  }

  crePagination(newPage: number, pageurl) {
    this.commonservice.getListData(this.srhList, newPage, this.pageSize, pageurl)
      .subscribe(
        data => {
          if (data.total > 0) {
            this.data = data.rows;
            this.rowTotal = data.total;
            this.currentpage = newPage;

            console.log(this.data);

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
      const pageurl = this.baseUrl + this.transferdataUrl;

      this.commonservice.getListData(this.srhList, newPage, this.pageSize, pageurl)
        .subscribe(
          data => {
            if (data.total > 0) {
              this.data = data.rows;
              this.rowTotal = data.total;
              this.currentpage = newPage;
              this.chkList = [];

              this.commonservice.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
            } else {
              this.data = null;
              this.rowTotal = 0;
              this.currentpage = 1;
              this.chkList = [];

              this.commonservice.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
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

  fileChange(e) {
    this.fileList = [];
    if (e.target.files[0] !== undefined) {
      this.fileList.push({ file: e.target.files[0] });
    }
  }

  doImport() {
    const schk1 = document.getElementById('chkfile1') as HTMLInputElement;
    const schk2 = document.getElementById('chkfile2') as HTMLInputElement;

    if (!schk1.checked && !schk2.checked) {
      return alert('必須選擇要重匯的文件類別！');
    } else if (this.chkList.length !== 1) {
      return alert('請選擇一筆資料！');
    } else if (this.fileList.length !== 1) {
      return alert('請選擇要匯入的檔案！');
    }

    let type = '';
    if (schk1.checked) {
      type = 'SHIPPINGFILE';
    } else {
      type = 'BROKERFILE';
    }

    const formdata = new FormData();
    formdata.append('type', type);
    formdata.append('id', this.chkList[0]);
    formdata.append('fileUpload', this.fileList[0].file);

    this.commonservice.Upload(formdata, this.baseUrl + this.importUrl)
      .subscribe(data => {
        if (data.status === '0') {
          alert(data.msg);
          this.crePagination(this.currentpage, this.baseUrl + this.transferdataUrl);
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

  doDelete() {
    const IsConfirm = confirm('確定要刪除？');
    if (IsConfirm === true) {
      if (this.chkList.length > 0) {
        this.commonservice.delData(this.chkList, this.baseUrl + this.delUrl)
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
