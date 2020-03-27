import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-entrustcus-import',
  templateUrl: './entrustcus-import.component.html',
  styleUrls: ['./entrustcus-import.component.css']
})
export class EntrustcusImportComponent implements OnInit {

  /* web api url */
  importUrl = '/api/Member/ImportCusShippingData';

  tempList: any = [];
  fileList: any = [];

  constructor(
    private commonservice: CommonService
  ) { }

  ngOnInit() {
  }

  fileChange(e) {
    this.fileList = [];
    for (const item of e.target.files) {
      this.fileList.push({ file: item });
    }

    console.log(this.fileList);
  }

  saveData() {
    if (this.fileList.length > 2) {
      return alert('最多只能上傳兩個檔案！');
    }

    if (this.fileList.length > 0) {
      const process = document.getElementById('process-msg') as HTMLDivElement;
      process.innerHTML = '<span>匯入中，請稍後片刻……</span>';

      const formdata = new FormData();
      // tslint:disable-next-line: prefer-for-of
      for (let i = 0; i < this.fileList.length; i++) {
        formdata.append('fileUpload' + i, this.fileList[i].file);
      }

      this.commonservice.Upload(formdata, this.importUrl)
        .subscribe(data => {
          process.innerHTML = '<span>' + data.msg + '</span>';
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
