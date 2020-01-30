import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';
import { TransferDInfoCombine } from 'src/app/_Helper/models';

@Component({
  selector: 'app-shipping-combine',
  templateUrl: './shipping-combine.component.html',
  styleUrls: ['./shipping-combine.component.css']
})
export class ShippingCombineComponent implements OnInit {

  dataForm: FormGroup;

  getdataUrl = '/api/Member/GetTransferData';
  saveUrl = '/api/Member/SaveShippingData';

  masterData: TransferDInfoCombine[];
  dataChange;
  idList;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.resetForm();
    this.getData();
  }

  resetForm() {
    this.dataForm = this.formBuilder.group({
      receiver: [''],
      receiveraddr: ['', Validators.required],
      declare: [''],
      declareaddr: [''],
      phone: ['', Validators.required],
      mobile: ['', Validators.required]
    });
  }

  getData() {
    this.idList = sessionStorage.getItem('transid');
    this.commonservice.getData2(this.idList, this.getdataUrl)
      .subscribe(data => {
        if (data.rows.length > 0) {
          this.masterData = data.rows;
        }
      },
        error => {
          console.log(error);
        });
  }

  saveData(form) {

  }

  doCancel() {
    sessionStorage.removeItem('transid');
    this.router.navigate(['/member/shipping/t1/0']);
  }

}
