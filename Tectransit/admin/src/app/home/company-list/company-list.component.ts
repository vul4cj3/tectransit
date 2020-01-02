import { Component, OnInit } from '@angular/core';
import { AccountInfo, RankAccMapInfo, DeclarantInfo } from 'src/app/_Helper/models';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { ModalService } from 'src/app/services/modal.service';
import { ConfirmService } from 'src/app/services/confirm.service';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent implements OnInit {
  /* Web api url*/
  private baseUrl = window.location.origin + '/api/UserHelp/';
  private dataUrl = 'GetTSCompanyListData';
  private enableUrl = 'EditTSCompanyEnableData';
  private accrankUrl = 'EditCompanyRankData';
  private decnrecUrl = 'GetDeclarantnReceiverData';

  tableTitle = ['#', '帳號', '姓名', 'Email', '註冊時間',
    '最後登入時間', '登入次數', '停用', '編輯', '權限設定', '申報人', '收件人'];
  data: AccountInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 10;
  srhForm: FormGroup;

  RAMapItem: RankAccMapInfo[];
  DecList: DeclarantInfo[];
  activeList: any = [];
  powerList: any = [];
  pUserid: string;
  pUsercode: string;
  chkNum = 1;

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService,
    private modalService: ModalService,
    private confirmService: ConfirmService
  ) { }

  ngOnInit() {
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
      susercode: '',
      susername: '',
      semail: ''
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

  doIsenable() {
    if (this.activeList.length > 0) {
      this.commonService.editEnableData(this.activeList, this.baseUrl + this.enableUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.crePagination(this.currentpage);
            this.activeList = [];
          } else {
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });

    } else { alert('頁面無項目變更！'); }
  }

  activeSelChange(val, Ischk) {
    this.chkNum = 0;
    this.activeList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.activeList.length === 0) {
      this.activeList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.activeList.push({ id: val, isenable: Ischk });
      }
    }

    // console.log(this.activeList);
  }

  /* Popup window function */
  openModal(id: string, code: string) {
    if (id === 'custom-modal-1') {
      this.pUsercode = code;
      this.commonService.getAllRank(code).subscribe(data => {
        if (data.status === '0') {
          this.RAMapItem = data.item;
        } else {
          this.RAMapItem = null;
        }
      }, error => {
        console.log(error);
      });
    } else {
      const type = id === 'custom-modal-2' ? 2 : 1;
      this.commonService.getDecnRecData(type, code, this.baseUrl + this.decnrecUrl)
        .subscribe(data => {
          if (data.status === '0') {
            this.DecList = data.item;
          } else {
            this.DecList = null;
          }
        },
          error => {
            console.log(error);
          });
    }

    this.modalService.open(id);
  }

  closeModal(id: string) {
    this.pUsercode = '';
    this.powerList = [];
    this.modalService.close(id);
  }

  powerSelChange(val, Ischk) {
    this.chkNum = 0;
    this.powerList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.powerList.length === 0) {
      this.powerList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.powerList.push({ id: val, isenable: Ischk });
      }
    }
  }

  savePowerData() {
    if (this.pUsercode !== '' && this.powerList.length > 0) {
      this.commonService.editPowerData(this.pUsercode, this.powerList, this.baseUrl + this.accrankUrl)
        .subscribe(data => {
          alert(data.msg);
        },
          error => {
            console.log(error);
          });
    } else {
      alert('頁面無數據被修改！');
    }
  }
}
