import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransferHInfo, TransferDInfo } from 'src/app/_Helper/models';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-shipping-edit',
  templateUrl: './shipping-edit.component.html',
  styleUrls: ['./shipping-edit.component.css']
})
export class ShippingEditComponent implements OnInit {

  /* Web api url*/
  private baseUrl = '/api/Member/';
  private dataUrl = 'GetACTransferData';

  shippingType;
  cateID;
  dataID;

  data: TransferHInfo;
  subdata: TransferDInfo[];

  constructor(
    public commonService: CommonService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.shippingType = this.route.snapshot.paramMap.get('type');
    this.cateID = this.route.snapshot.paramMap.get('code');
    this.dataID = this.route.snapshot.paramMap.get('id');

    this.getData();
  }

  getData() {
    this.commonService.getData(this.dataID, this.baseUrl + this.dataUrl)
      .subscribe(
        data => {
          this.data = data.rows;
          this.subdata = data.subitem;
        },
        error => {
          console.log(error);
        });
  }

}
