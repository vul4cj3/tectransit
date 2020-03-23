import { Component, OnInit, Input } from '@angular/core';
import { BannerInfo } from '../../_Helper/models';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.css']
})
export class BannerComponent implements OnInit {

  dataUrl = '/api/CommonHelp/GetBanner';

  data: BannerInfo[];

  constructor(
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.commonservice.getSingleData(this.dataUrl)
      .subscribe(result => {
        this.data = result.dataList;
      }, error => {
        console.log(error);
      });

  }

}
