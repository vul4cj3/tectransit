import { Component, OnInit, OnChanges } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { RoleInfo, MenuInfo } from 'src/app/_Helper/models';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})
export class RoleListComponent implements OnInit {
  /* Web api url*/
  private baseUrl = window.location.origin + '/api/SysHelp/';
  private dataUrl = 'GetTSRoleListData';
  private data2Url = 'GetTSRoleListData_C';
  private enableUrl = 'EditTSRoleEnableData';
  private rolemenuUrl = 'EditRoleMenuData';

  tableTitle = ['#', '代碼', '名稱', '敘述', '建立時間',
    '建立者', '更新時間', '更新者', '停用', '編輯', '選單權限'];
  data: RoleInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 10;
  srhForm: FormGroup;
  pageUrl: string;

  menuItem: MenuInfo[];
  menuSubItem: MenuInfo[];
  activeList: any = [];
  powerList: any = [];
  pRolecode: string;
  chkNum = 1;

  isSuper = false; // 系統管理者才可瀏覽&編輯

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService,
    private modalService: ModalService
  ) { }

  ngOnInit() {

    this.resetData();

  }

  searchData() {
    this.currentpage = 1;
    this.getData();
  }

  resetData() {
    // built form controls and default form value
    this.srhForm = this.formBuilder.group({
      srolecode: '',
      srolename: ''
    });

    this.commonService.chkIsSuper()
      .subscribe((data) => {
        this.isSuper = data;
        this.getData();
      });
  }

  getData() {
    if (this.isSuper) {
      this.crePagination(this.currentpage, this.baseUrl + this.dataUrl);
    } else {
      this.crePagination(this.currentpage, this.baseUrl + this.data2Url);
    }
  }

  crePagination(newPage: number, pageUrl: string) {
    this.commonService.getListData(this.srhForm.value, newPage, this.pageSize, pageUrl)
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

    if (this.isSuper) {
      this.pageUrl = this.baseUrl + this.dataUrl;
    } else {
      this.pageUrl = this.baseUrl + this.data2Url;
    }

    if (newPage !== this.currentpage) {
      this.commonService.getListData(this.srhForm.value, newPage, this.pageSize, this.pageUrl)
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
            this.getData();
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
  }

  /* Popup window function */
  openModal(id: string, code: string) {
    this.pRolecode = code;
    this.commonService.getAllMenu(code, '1').subscribe(data => {
      if (data.status === '0') {
        this.menuItem = data.pList;
        this.menuSubItem = data.item;
      } else {
        this.menuItem = null;
        this.menuSubItem = null;
      }
    }, error => {
      console.log(error);
    });

    this.modalService.open(id);
  }

  closeModal(id: string) {
    this.pRolecode = '';
    this.powerList = [];
    this.modalService.close(id);
  }

  // Select All Onchange
  selAllChange(val, Ischk) {
    for (let i = 0; i < this.menuSubItem.length; i++) {
      const subChk = document.getElementById('menu' + i);
      if (subChk.getAttribute('value').substring(0, 2) === val) {
        this.powerSelChange(subChk.getAttribute('value'), Ischk);
        if (Ischk) {
          (subChk as HTMLInputElement).checked = true;
        } else {
          (subChk as HTMLInputElement).checked = false;
        }
      }
    }
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
    if (this.pRolecode !== '' && this.powerList.length > 0) {
      this.commonService.editPowerData(this.pRolecode, this.powerList, this.baseUrl + this.rolemenuUrl)
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
