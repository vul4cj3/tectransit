import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { ShippingMCusInfo, ShippingHCusInfo, ShippingDCusInfo, DeclarantCusInfo } from 'src/app/_Helper/models';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-shippingcus-edit',
  templateUrl: './shippingcus-edit.component.html',
  styleUrls: ['./shippingcus-edit.component.css']
})
export class ShippingcusEditComponent implements OnInit {

  /* Web api url */
  private baseUrl = window.location.origin + '/api/UserHelp/';
  private dataUrl = 'GetSingleShippingCusData';
  private dataEditUrl = 'EditShippingCusData';

  dataChange;
  dataForm: FormGroup;
  shippingMID = '';
  masterData: ShippingMCusInfo;
  headerData: ShippingHCusInfo[];
  detailData: ShippingDCusInfo[];
  declarData: DeclarantCusInfo[];

  isModify = false;
  cusStatus;
  isErr = false;

  ismult: boolean;

  hiddellist: any = [];
  diddellist: any = [];
  deciddellist: any = [];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    // get parameters id then get data
    this.shippingMID = this.route.snapshot.paramMap.get('id');

    if (this.shippingMID !== '0') {
      this.resetForm();
      this.getData();
    }
  }

  resetForm() {
    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      id: 0,
      total: ['0', Validators.required],
      receiver: [''],
      receiveraddr: [''],
      mawbno: '',
      clearanceno: '',
      hawbno: '',
      ismultreceiver: 'N',
      status: '1',
      boxform: this.formBuilder.array([
        // this.initBox()
      ]),
      decform: this.formBuilder.array([
        // this.initDec()
      ])
    });
  }

  getData() {
    this.commonService.getSingleData(this.shippingMID, this.baseUrl + this.dataUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.dataForm.patchValue(data.rowM);
          this.masterData = data.rowM;
          this.headerData = data.rowH;
          this.detailData = data.rowD;
          this.declarData = data.rowDec;

          this.ismult = this.masterData.ismultreceiver === 'Y' ? true : false;
        }
      },
        error => {
          console.log(error);
        });

    this.cusStatus = sessionStorage.getItem('cusstatus');
  }

  chgDetail(type) {
    if (type === 'm') {
      this.isModify = true;
      this.readBox();
      this.readDec();

    } else {
      this.isModify = false;
      this.hiddellist = [];
      this.diddellist = [];
      this.deciddellist = [];
      this.ismult = this.masterData.ismultreceiver === 'Y' ? true : false;
      this.resetForm();
      this.dataForm.patchValue(this.masterData);
    }
  }

  chgRec(ischk) {

    // 檢查contols
    if (ischk) {
      const control = this.dataForm.get('boxform') as FormArray;
      for (let i = 0; i < control.length; i++) {
        const temp = control.controls[i] as FormGroup;
        if (temp.controls.receiver === undefined) {
          temp.addControl('receiver', new FormControl(''));
        }
        if (temp.controls.receiveraddr === undefined) {
          temp.addControl('receiveraddr', new FormControl(''));
        }
      }
      control.patchValue(this.headerData);
    } else {
      const control = this.dataForm.get('boxform') as FormArray;
      for (let i = 0; i < control.length; i++) {
        const temp = control.controls[i] as FormGroup;
        if (temp.controls.receiver !== undefined) {
          temp.removeControl('receiver');
        }
        if (temp.controls.receiveraddr !== undefined) {
          temp.removeControl('receiveraddr');
        }
      }
    }

    this.dataForm.controls.ismultreceiver.setValue((ischk ? 'Y' : 'N'));
    this.ismult = ischk;
    // this.masterData.ismultreceiver = (ischk);
  }

  //#region Modify Detail CRUD

  initBox() {
    if (this.masterData.ismultreceiver) {
      return this.formBuilder.group({
        id: [0],
        boxno: ['', [Validators.required]],
        productform: this.formBuilder.array([
          this.initProduct()
        ]),
        receiver: [''],
        receiveraddr: ['']
      });
    } else {
      return this.formBuilder.group({
        id: [0],
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
      id: [0],
      product: ['', Validators.required],
      quantity: ['', Validators.required],
      unitprice: ['', Validators.required],
      // ---------------------------------------------------------------------
    });
  }

  initDec() {
    return this.formBuilder.group({
      //  ---------------------forms fields ------------------------
      id: [0],
      name: ['', Validators.required],
      taxid: ['', Validators.required],
      phone: [''],
      mobile: [''],
      addr: [''],
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

  removebox(rowIndex) {
    const rows = this.dataForm.controls.boxform as FormArray;
    const row = rows.controls[rowIndex] as FormGroup;
    if (row.controls.id.value !== 0) {
      this.hiddellist.push(row.controls.id.value);
    }
    rows.removeAt(rowIndex);
  }

  removeprd(rowIndex, subrowIndex) {
    const rows = (this.dataForm.controls.boxform as FormArray).at(rowIndex).get('productform') as FormArray;
    const row = rows.controls[subrowIndex] as FormGroup;
    if (row.controls.id.value !== 0) {
      this.diddellist.push(row.controls.id.value);
    }
    rows.removeAt(subrowIndex);
  }

  removedec(rowIndex) {
    const rows = this.dataForm.controls.decform as FormArray;
    const row = rows.controls[rowIndex] as FormGroup;
    if (row.controls.id.value !== 0) {
      this.deciddellist.push(row.controls.id.value);
    }
    rows.removeAt(rowIndex);

    // this.refreshImg(rowIndex);
    // this.refreshFile(rowIndex);
  }

  readBox() {
    const control = this.dataForm.controls.boxform as FormArray;
    if (this.headerData.length > 0) {
      // tslint:disable-next-line: prefer-for-of
      for (let i = 0; i < this.headerData.length; i++) {
        if (this.ismult) {
          control.push(this.formBuilder.group({
            id: [this.headerData[i].id],
            boxno: [this.headerData[i].boxno, [Validators.required]],
            productform: this.formBuilder.array([
            ]),
            receiver: [this.headerData[i].receiver],
            receiveraddr: [this.headerData[i].receiveraddr]
          }));
        } else {
          control.push(this.formBuilder.group({
            id: [this.headerData[i].id],
            boxno: [this.headerData[i].boxno, [Validators.required]],
            productform: this.formBuilder.array([
            ])
          }));
        }
      }

      for (let j = 0; j < this.headerData.length; j++) {
        const control2 = control.at(j).get('productform') as FormArray;
        // tslint:disable-next-line: prefer-for-of
        for (let k = 0; k < this.detailData.length; k++) {
          if (this.headerData[j].id === this.detailData[k].shippingiD_H) {
            control2.push(this.formBuilder.group({
              id: [this.detailData[k].id],
              product: [this.detailData[k].product, Validators.required],
              quantity: [this.detailData[k].quantity, Validators.required],
              unitprice: [this.detailData[k].unitprice, Validators.required],
            }));
          }
        }
      }
    }

  }

  // read declarant data
  readDec() {
    const control = this.dataForm.controls.decform as FormArray;
    if (this.declarData.length > 0) {
      for (let i = 0; i < this.declarData.length; i++) {
        if (control.at(i) !== undefined) {
          control.at(i).patchValue(this.declarData[i]);
        } else {
          control.push(this.formBuilder.group({
            id: [this.declarData[i].id],
            name: [this.declarData[i].name, Validators.required],
            taxid: [this.declarData[i].taxid, Validators.required],
            phone: [this.declarData[i].phone],
            mobile: [this.declarData[i].mobile],
            addr: [this.declarData[i].addr],
            idphotof: [this.declarData[i].idphotof],
            idphotob: [this.declarData[i].idphotob],
            appointment: [this.declarData[i].appointment]
          }));
        }

      }
    }
  }
  //#endregion

  saveData(form) {
    // check Form
    if (this.dataForm.invalid) {
      return alert('必填不能為空！');
    }

    const postData = { delH: this.hiddellist, delD: this.diddellist, delDec: this.deciddellist, formdata: form };
    this.commonService.editShippingData(postData, this.baseUrl + this.dataEditUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.getData();
          alert(data.msg);
        } else {
          alert(data.msg);
        }
      },
        error => {
          console.log(error);
        });

  }


}
