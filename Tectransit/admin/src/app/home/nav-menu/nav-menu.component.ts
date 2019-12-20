import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})
export class NavMenuComponent implements OnInit {

  activeNav: Array<boolean> = [];
  menuItem: MenuInfo[];
  menuSubItem: MenuInfo[];

  constructor(private commonService: CommonService) {
   }

  ngOnInit() {
    this.commonService.getMenu(sessionStorage.getItem('currentUser'))
    .subscribe(
      data => {
        if (data.status === '0') {
          this.menuItem = data.pList;
          this.menuSubItem = data.item;
        } else {
          this.menuItem = null;
          this.menuSubItem = null;
        }
      },
      error => {
        console.log(error);
      });
  }
}

interface MenuInfo {
  menucode: string;
  parentcode: string;
  menuurl: string;
  menuname: string;
  iconurl: string;
}
