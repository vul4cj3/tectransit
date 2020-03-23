import { Component, OnInit } from '@angular/core';
import { CommonService } from '../../services/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AboutInfo } from '../../_Helper/models';

@Component({
  selector: 'app-custom-detail',
  templateUrl: './custom-detail.component.html',
  styleUrls: ['./custom-detail.component.css']
})
export class CustomDetailComponent implements OnInit {

  /* web api url */
  dataUrl = '/api/FrontHelp/GetAboutData';

  id;
  data: AboutInfo = null;

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
          this.router.navigate(['/custom']);
        } else {
          this.data = result.rows;
        }
      }, error => {
        console.log(error);
      });
  }

}
