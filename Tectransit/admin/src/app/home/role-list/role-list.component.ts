import { Component, OnInit } from '@angular/core';
import { SysService } from 'src/app/services/sys.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})
export class RoleListComponent implements OnInit {

  data: RoleInfo[];
  rowTotal = 0;
  currentpage = 0;
  pageSize = 10;

  constructor(
    private pageService: CommonService,
    private sysService: SysService) { }

  ngOnInit() {
    this.sysService.getRoleLitData(this.currentpage, this.pageSize).subscribe(
      data => {
        if (data.total > 0) {
          this.data = data.rows;
          this.rowTotal = data.total;
          this.currentpage = this.currentpage;

          this.pageService.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
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
        this.sysService.getRoleLitData(newPage, this.pageSize).subscribe(
          data => {
            if (data.total > 0) {
              this.data = data.rows;
              this.rowTotal = data.total;
              this.currentpage = newPage;

              this.pageService.set_pageNumArray(this.rowTotal, this.pageSize, this.currentpage);
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

}

interface RoleInfo {
  roleid: number;
  rolecode: string;
  rolename: string;
  roledesc: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}
