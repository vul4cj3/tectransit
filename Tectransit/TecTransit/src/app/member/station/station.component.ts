import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { MemStationInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-station',
  templateUrl: './station.component.html',
  styleUrls: ['./station.component.css']
})
export class StationComponent implements OnInit {

  getdataUrl = '/api/Member/GetStationData';
  stationData: MemStationInfo;

  memName: string;
  warehouseNo: string;

  constructor(
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.commonservice.getSingleData(this.getdataUrl)
      .subscribe(data => {
        this.stationData = data.rows;
        this.memName = data.rows[0].username;
        this.warehouseNo = data.rows[0].warehouseno;
      }, error => {
        console.log(error);
      });
  }

}
