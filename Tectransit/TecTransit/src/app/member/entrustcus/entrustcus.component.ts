import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-entrustcus',
  templateUrl: './entrustcus.component.html',
  styleUrls: ['./entrustcus.component.css']
})
export class EntrustcusComponent implements OnInit {
  dataForm: FormGroup;
  stationData;

  IsmultRec: boolean;

  getStationUrl = '/api/Member/GetStationData';
  saveUrl = '/api/Member/SaveCusShippingData';

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
    const chk = document.getElementById('IsmultReceiver') as HTMLInputElement;
    chk.checked = false;
    this.IsmultRec = false;

    this.dataForm = this.formBuilder.group({
      stationcode: [''],
      trasferno: ['', Validators.required],
      total: ['', Validators.required],
      receiver: ['', Validators.required],
      receiveraddr: ['', Validators.required],
      boxform: this.formBuilder.array([
        this.initBox()
      ]),
      decform: this.formBuilder.array([
        this.initDec()
      ])
    });
  }

  //#region Form CRUD function
  initBox() {
    if (this.IsmultRec) {
      return this.formBuilder.group({
        boxno: ['', [Validators.required]],
        productform: this.formBuilder.array([
          this.initProduct()
        ]),
        receiver: ['', [Validators.required]],
        receiveraddr: ['', [Validators.required]]
      });
    } else {
      return this.formBuilder.group({
        boxno: ['', [Validators.required]],
        productform: this.formBuilder.array([
          this.initProduct()
        ])
      });
    }
  }

  initProduct() {
    return this.formBuilder.group({
      //  ---------------------forms fields ------------------------
      product: ['', [Validators.required]],
      quantity: ['', [Validators.required, Validators.pattern('[0-9]{9}')]],
      unitprice: ['', [Validators.required, Validators.pattern('[0-9]{10}')]],
      // ---------------------------------------------------------------------
    });
  }

  initDec() {
    return this.formBuilder.group({
      //  ---------------------forms fields ------------------------
      name: ['', [Validators.required]],
      taxid: ['', [Validators.required]],
      phone: [''],
      mobile: [''],
      addr: ['', [Validators.required]],
      idphotof: [''],
      idphotob: [''],
      appointment: [''],
      // ---------------------------------------------------------------------
    });
  }

  addbox() {
    const control = this.dataForm.controls.boxform as FormArray;
    control.push(this.initBox());
  }

  addprd(ibox) {
    const control = (this.dataForm.controls.boxform as FormArray).at(ibox).get('productform') as FormArray;
    control.push(this.initProduct());
  }

  adddec() {
    const control = this.dataForm.controls.decform as FormArray;
    control.push(this.initDec());
  }

  addrec() {
    if (this.IsmultRec) {
      const control = this.dataForm.get('boxform') as FormArray;
      for (let i = 0; i < control.length; i++) {
        const temp = control.controls[i] as FormGroup;
        temp.addControl('receiver', new FormControl('', Validators.required));
        temp.addControl('receiveraddr', new FormControl('', Validators.required));
      }
    } else {
      this.dataForm.addControl('receiver', new FormControl('', Validators.required));
      this.dataForm.addControl('receiveraddr', new FormControl('', Validators.required));
    }
  }

  removebox(rowIndex) {
    const rows = this.dataForm.controls.boxform as FormArray;
    rows.removeAt(rowIndex);
  }

  removeprd(rowIndex, subrowIndex) {
    const rows = (this.dataForm.controls.boxform as FormArray).at(rowIndex).get('productform') as FormArray;
    rows.removeAt(subrowIndex);
  }

  removedec(rowIndex) {
    const rows = this.dataForm.controls.decform as FormArray;
    rows.removeAt(rowIndex);
  }

  removerec() {
    if (this.IsmultRec) {
      this.dataForm.removeControl('receiver');
      this.dataForm.removeControl('receiveraddr');
    } else {
      const control = this.dataForm.get('boxform') as FormArray;
      for (let i = 0; i < control.length; i++) {
        const temp = control.controls[i] as FormGroup;
        temp.removeControl('receiver');
        temp.removeControl('receiveraddr');
      }
    }
  }
  //#endregion

  chgReciver() {
    const chk = document.getElementById('IsmultReceiver') as HTMLInputElement;
    this.IsmultRec = chk.checked;
    this.removerec();
    this.addrec();
  }

  getData() {
    this.commonservice.getSingleData(this.getStationUrl)
      .subscribe(data => {
        this.stationData = data.rows;
      }, error => {
        console.log(error);
      });
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
