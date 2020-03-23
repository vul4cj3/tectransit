import { Component, OnInit } from '@angular/core';
import { MenuInfo } from '../../_Helper/models';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {

  sitmapData: MenuInfo[];

  constructor(private commonservice: CommonService) { }

  ngOnInit() {
    this.getSiteMap();
  }

  getSiteMap() {
    this.commonservice.getMenu()
      .subscribe(data => {
        this.sitmapData = data.pList;
      }, error => {
        console.log(error);
      });
  }

}
