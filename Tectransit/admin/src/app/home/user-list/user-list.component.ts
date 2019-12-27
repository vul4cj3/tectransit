import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { UserInfo, RoleUserMapInfo } from 'src/app/_Helper/models';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ModalService } from 'src/app/services/modal.service';
import { ConfirmService } from 'src/app/services/confirm.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  /* Web api url*/
  private baseUrl = window.location.origin + '/api/SysHelp/';
  private dataUrl = 'GetTSUserListData';
  private enableUrl = 'EditTSUserEnableData';
  private userroleUrl = 'EditUserRoleData';

  tableTitle = ['#', '代碼', '名稱', '敘述', '建立時間',
    '建立者', '更新時間', '更新者', '停用', '編輯', '權限設定', '重置密碼'];
  data: UserInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 10;
  srhForm: FormGroup;

  RUMapItem: RoleUserMapInfo[];
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
    // built form controls and default form value
    this.srhForm = this.formBuilder.group({
      susercode: '',
      susername: ''
    });

    this.crePagination(this.currentpage);
  }

  searchData() {
    this.currentpage = 1;
    this.crePagination(this.currentpage);
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
          alert(data.msg);
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

    console.log(this.activeList);
  }

  /* Popup window function */
  openModal(id: string, code: string) {
    this.pUsercode = code;
    this.commonService.getAllRole(code).subscribe(data => {
      if (data.status === '0') {
        this.RUMapItem = data.item;
      } else {
        this.RUMapItem = null;
      }
    }, error => {
      console.log(error);
    });

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
      this.commonService.editPowerData(this.pUsercode, this.powerList, this.baseUrl + this.userroleUrl)
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

  openComfirm(id: string, userid: string, code: string) {
    this.pUserid = userid;
    this.pUsercode = code;
    this.confirmService.open(id);
  }

  closeComfirm(id: string) {
    this.pUserid = '';
    this.pUsercode = '';
    this.confirmService.close(id);
  }

  resetData(id: string) {
    this.confirmService.close(id);
    this.commonService.resetPW(this.pUserid)
      .subscribe(data => {
        alert(data.msg);
      },
        error => {
          console.log(error);
        });

  }

}
