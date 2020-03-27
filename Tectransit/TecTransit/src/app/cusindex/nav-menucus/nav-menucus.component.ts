import { Component, OnInit } from '@angular/core';
import { CusloginService } from 'src/app/services/cuslogin.service';
import { MenuInfo } from 'src/app/_Helper/models';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-nav-menucus',
  templateUrl: './nav-menucus.component.html',
  styleUrls: ['./nav-menucus.component.css']
})
export class NavMenucusComponent implements OnInit {

  menuUrl = '/api/CommonHelp/GetCusMenu';

  data: MenuInfo[];

  constructor(
    private cusloginservice: CusloginService,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.GetMenuData();
  }

  GetMenuData() {
    this.commonservice.getData('', this.menuUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.data = data.pList;
        }
      }, error => {
        console.log(error);
      });
  }

  doLogout() {
    this.cusloginservice.logout();
    // redirect to login page
    document.location.href = '/cuslogin';
  }

}
