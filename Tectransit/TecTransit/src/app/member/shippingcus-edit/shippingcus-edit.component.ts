import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute } from '@angular/router';
import { ShippingMCusInfo, ShippingHCusInfo, ShippingDCusInfo, DeclarantCusInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-shippingcus-edit',
  templateUrl: './shippingcus-edit.component.html',
  styleUrls: ['./shippingcus-edit.component.css']
})
export class ShippingcusEditComponent implements OnInit {

  /* Web api url*/
  private baseUrl = '/api/Member/';
  private dataUrl = 'GetSingleShippingCusData';

  shippingType;
  cateID;
  dataID;

  masterData: ShippingMCusInfo;
  headerData: ShippingHCusInfo[];
  detailData: ShippingDCusInfo[];
  declarData: DeclarantCusInfo[];

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
          if (data.status === '0') {
            this.masterData = data.rowM;
            this.headerData = data.rowH;
            this.detailData = data.rowD;
            this.declarData = data.rowDec;
          }
        },
        error => {
          console.log(error);
        });
  }

}
