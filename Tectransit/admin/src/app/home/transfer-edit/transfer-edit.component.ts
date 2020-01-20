import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';
import { TransferHInfo, TransferDInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-transfer-edit',
  templateUrl: './transfer-edit.component.html',
  styleUrls: ['./transfer-edit.component.css']
})
export class TransferEditComponent implements OnInit {

  /* Web api url */
  private baseUrl = window.location.origin + '/api/UserHelp/';
  private dataUrl = 'GetTransferData';
  private dataEditUrl = 'EditTransferData';

  dataChange;
  dataForm: FormGroup;
  transferID = '';
  transferHData: TransferHInfo;
  transferDData: TransferDInfo[];
  isErr = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      transid: 0,
      trasferno: '',
      stationname: '',
      accountcode: '',
      trasfercompany: '',
      plength: '0',
      pwidth: '0',
      pheight: '0',
      pweight: '0',
      status: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: ''
    });

    // get parameters id then get data
    this.transferID = this.route.snapshot.paramMap.get('id');

    if (this.transferID !== '0') {
      this.getData(this.transferID);
    }
  }

  getData(id) {
    this.commonService.getSingleData(id, this.baseUrl + this.dataUrl)
      .subscribe(data => {
        this.dataForm.controls.trasferno.disable();
        this.dataForm.controls.stationname.disable();
        this.dataForm.controls.accountcode.disable();
        this.dataForm.patchValue(data.rows);
        this.transferHData = data.rows;
        this.transferDData = data.subitem;
      },
        error => {
          console.log(error);
        });
  }

  saveData(form) {
    // check Form
    if (this.dataForm.invalid) {
      this.isErr = true;
      return alert('標題不能為空！');
    }

    if (this.transferID !== '0') {
      this.dataChange = this.commonService.formChanges(form, this.transferHData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: this.transferID, formdata: this.dataChange };
      this.commonService.editSingleData(postData, this.baseUrl + this.dataEditUrl)
        .subscribe(data => {
          alert(data.msg);
        },
          error => {
            console.log(error);
          });
    } else { alert('頁面無數據被修改！'); }
  }


}
