import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-entrust',
  templateUrl: './entrust.component.html',
  styleUrls: ['./entrust.component.css']
})
export class EntrustComponent implements OnInit {
  dataForm: FormGroup;
  stationData;

  getStationUrl = '/api/Member/GetStationData';
  saveUrl = '/api/Member/SaveTansferData';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private commonservice: CommonService
  ) {

    this.resetForm();
    this.initGroup();
  }

  ngOnInit() {
    this.getData();
  }

  resetForm() {
    this.dataForm = this.formBuilder.group({
      stationcode: [''],
      trasferno: ['', Validators.required],
      rows: this.formBuilder.array([])
    });
  }

  getData() {
    this.commonservice.getSingleData(this.getStationUrl)
      .subscribe(data => {
        this.stationData = data.rows;
      }, error => {
        console.log(error);
      });
  }

  initGroup() {
    const rows = this.dataForm.get('rows') as FormArray;
    rows.push(this.formBuilder.group({
      product: ['', Validators.required],
      quantity: ['', Validators.required],
      price: ['', Validators.required]
    }));
  }

  onDeleteRow(rowIndex) {
    const rows = this.dataForm.get('rows') as FormArray;
    rows.removeAt(rowIndex);
  }

  SaveData(form) {
    if (form.stationcode === '') {
      return alert('必須選擇集貨站！');
    }

    if (this.dataForm.invalid) {
      return alert('欄位不能為空白！');
    }


    if (Object.keys(form).length > 0) {
      this.commonservice.insertData(form, this.saveUrl)
        .subscribe(data => {
          if (data.status === '0') {
            this.resetForm();
            alert(data.msg);
          } else {
            alert(data.msg);
          }

        }, error => {
          console.log(error);
        });

    }
  }
}
