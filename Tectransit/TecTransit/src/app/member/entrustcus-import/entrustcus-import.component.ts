import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-entrustcus-import',
  templateUrl: './entrustcus-import.component.html',
  styleUrls: ['./entrustcus-import.component.css']
})
export class EntrustcusImportComponent implements OnInit {

  /* web api url */
  getStationUrl = '/api/Member/GetStationData';
  importUrl = '/api/Member/ImportCusShippingData';

  stationData;

  tempList: any = [];
  fileList: any = [];

  constructor(
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.commonservice.getSingleData(this.getStationUrl)
      .subscribe(data => {
        this.stationData = data.rows;
      }, error => {
        console.log(error);
      });
  }

  fileChange(e) {
    this.fileList.push({ file: e.target.files[0] });
  }

  saveData() {
    if (this.fileList.length > 0) {
      const formdata = new FormData();
      const scode = document.getElementById('stationcode') as HTMLSelectElement;
      formdata.append('stationcode', scode.value);
      formdata.append('fileUpload', this.fileList[0].file);

      this.commonservice.Upload(formdata, this.importUrl)
        .subscribe(data => {
          alert(data.msg);
        },
          error => {
            console.log(error);
          });
    } else {
      alert('請選擇要上傳的檔案！');
    }
  }

}
