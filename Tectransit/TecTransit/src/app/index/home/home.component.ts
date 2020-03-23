import { Component, OnInit } from '@angular/core';
import { NewsInfo } from '../../_Helper/models';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  newsurl = '/api/FrontHelp/GetNewsData';

  public title = 'TecTransit';

  newsData: NewsInfo[];

  constructor(
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.getNewsData();
  }

  getNewsData() {
    this.commonservice.getSingleData(this.newsurl)
      .subscribe(data => {
        if (data.status === '0') {
          this.newsData = data.row;
        }
      },
        error => {
          console.log(error);
        });
  }

}
