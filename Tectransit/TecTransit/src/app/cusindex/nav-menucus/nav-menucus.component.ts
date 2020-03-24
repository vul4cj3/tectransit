import { Component, OnInit } from '@angular/core';
import { CusloginService } from 'src/app/services/cuslogin.service';

@Component({
  selector: 'app-nav-menucus',
  templateUrl: './nav-menucus.component.html',
  styleUrls: ['./nav-menucus.component.css']
})
export class NavMenucusComponent implements OnInit {

  constructor(
    private cusloginservice: CusloginService
  ) { }

  ngOnInit() {
  }

  doLogout() {
    this.cusloginservice.logout();
    // redirect to login page
    document.location.href = '/cuslogin';
  }

}
