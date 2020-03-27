import { Component, OnInit } from '@angular/core';
import { RankInfo, MenuInfo } from 'src/app/_Helper/models';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-rank-list',
  templateUrl: './rank-list.component.html',
  styleUrls: ['./rank-list.component.css']
})
export class RankListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = window.location.origin + '/api/UserHelp/';
  private dataUrl = 'GetTSRankListData';
  private enableUrl = 'EditTSRankEnableData';
  private rankmenuUrl = 'EditRankMenuData';

  tableTitle = ['#', '代碼', '名稱', '敘述', '建立時間',
    '建立者', '更新時間', '更新者', '停用', '編輯', '選單權限'];
  data: RankInfo[];
  rowTotal = 0;
  currentpage = 1;
  pageSize = 10;
  srhForm: FormGroup;

  activeList: any = [];
  menuItem: MenuInfo[];
  menuSubItem: MenuInfo[];
  powerList: any = [];
  pRolecode: string;
  chkNum = 1;

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService,
    private modalService: ModalService
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
      srankcode: '',
      srankname: '',
      sranktype: '1'
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
  }

  /* Popup window function */
  openModal(id: string, rankid: string) {
    this.pRolecode = rankid;
    this.commonService.getAllMenu(rankid, '0').subscribe(data => {
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
      if (subChk.getAttribute('value').substring(0, 3) === val) {
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
      this.commonService.editPowerData(this.pRolecode, this.powerList, this.baseUrl + this.rankmenuUrl)
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
