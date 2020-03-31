import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { ShippingMCusInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-imbroker-list',
  templateUrl: './imbroker-list.component.html',
  styleUrls: ['./imbroker-list.component.css']
})
export class ImbrokerListComponent implements OnInit {

  private baseUrl = '/api/broker/';
  private dataUrl = 'GetShippingCusIMBRData';
  private CusBrokerUrl = 'ExportBroker';
  private NewCusBrokerUrl = 'ExportNewBroker';

  tableTitle = ['集運單號', '已消倉表', '新已消倉表', 'MAWB', '建單時間'];

  data: ShippingMCusInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 20;

  srhList: any = [];

  constructor(
    public commonservice: CommonService
  ) { }

  ngOnInit() {
    this.crePagination(this.currentpage, this.baseUrl + this.dataUrl);
  }

  srhData() {
    this.srhList = [];
    const sdate = document.getElementById('credates') as HTMLInputElement;
    const edate = document.getElementById('credatee') as HTMLInputElement;

    this.srhList.push({
      cresdate: sdate.value, creedate: edate.value
    });

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

      this.commonservice.getListData(this.srhList, newPage, this.pageSize, this.baseUrl + this.dataUrl)
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

  // 轉出消倉表
  exportCusBroker(id) {
    this.commonservice.getData(id, this.baseUrl + this.CusBrokerUrl)
      .subscribe(res => {
        if (res.status === 0) {
          return window.open(res.msg);
        } else {
          return alert(res.msg);
        }
      }, error => {
        console.log(error);
      });
  }

  // 轉出新版消倉表
  exportNewCusBroker(id) {
    this.commonservice.getData(id, this.baseUrl + this.NewCusBrokerUrl)
      .subscribe(res => {
        if (res.status === 0) {
          return window.open(res.msg);
        } else {
          return alert(res.msg);
        }
      }, error => {
        console.log(error);
      });
  }

}
