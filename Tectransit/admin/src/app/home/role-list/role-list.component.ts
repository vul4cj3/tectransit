import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { RoleInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})
export class RoleListComponent implements OnInit {

  /* Web api url*/
  private baseUrl = window.location.origin + '/api/SysHelp/';
  private roleUrl = 'GetTSRoleListData';
  private enableUrl = 'EditTSRoleEnableData';

  tableTitle = ['#', '代碼', '名稱', '敘述', '建立時間',
    '建立者', '更新時間', '更新者', '停用', '編輯'];
  data: RoleInfo[];
  rowTotal = 0;
  currentpage = 0;
  pageSize = 10;

  activeList: any = [];
  chkNum = 1;

  constructor(
    private commonService: CommonService
  ) { }

  ngOnInit() {
    this.crePagination(this.currentpage);

  }

  crePagination(newPage: number) {
    this.commonService.getListData(newPage, this.pageSize, this.baseUrl + this.roleUrl)
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
            this.currentpage = 0;
          }
        },
        error => {
          console.log(error);
        });
  }

  changeData(newPage: number) {
    // 重新取得Role資料
    this.data = this.data.map((item) => {
      if (newPage !== this.currentpage) {
        this.commonService.getListData(newPage, this.pageSize, this.baseUrl + this.roleUrl)
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
                this.currentpage = 0;
              }
            },
            error => {
              console.log(error);
            });
      }
      return item;
    });
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
  }

}
