import { Component, OnInit, Pipe, PipeTransform } from '@angular/core';
import { CommonService } from '../services/common.service';
import { NewsInfo } from '../_Helper/models';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-news-detail',
  templateUrl: './news-detail.component.html',
  styleUrls: ['./news-detail.component.css']
})
export class NewsDetailComponent implements OnInit {

  /* web api url */
  dataUrl = '/api/FrontHelp/GetNews';

  id;
  data: NewsInfo = null;

  constructor(
    private commonservice: CommonService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getData(this.id);
  }

  getData(id) {
    this.commonservice.getData(id, this.dataUrl)
      .subscribe(result => {
        if (result.rows === '') {
          this.router.navigate(['/news']);
        } else {
          this.data = result.rows;
        }
      }, error => {
        console.log(error);
      });
  }

}
