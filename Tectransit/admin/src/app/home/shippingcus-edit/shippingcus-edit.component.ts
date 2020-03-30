import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { ShippingMCusInfo, ShippingHCusInfo, ShippingDCusInfo, DeclarantCusInfo, AccountInfo } from 'src/app/_Helper/models';
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
  private brokerdataUrl = 'GetBrokerData';
  private dataEditUrl = 'EditShippingCusData';

  dataChange;
  dataForm: FormGroup;
  shippingMID = '';
  masterData: ShippingMCusInfo;
  headerData: ShippingHCusInfo[];
  detailData: ShippingDCusInfo[];
  declarData: DeclarantCusInfo[];

  imBroker: AccountInfo[];
  exBroker: AccountInfo[];

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
      this.getEditData();
    }
  }

  resetForm() {
    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      id: 0,
      mawbno: ['0', Validators.required],
      flightnum: [''],
      storecode: [''],
      total: ['0', Validators.required],
      totalweight: ['0', Validators.required],
      shippercompany: [''],
      shipper: [''],
      receivercompany: [''],
      receiver: [''],
      receiverzipcode: [''],
      receiveraddr: [''],
      receiverphone: [''],
      receivertaxid: [''],
      ismultreceiver: 'N',
      status: '1',
      exbrokerid: '0',
      imbrokerid: '0',
      boxform: this.formBuilder.array([
        // this.initBox()
      ])
      // decform: this.formBuilder.array([
      //   // this.initDec()
      // ])
    });
  }

  getEditData() {
    this.commonService.getData(this.baseUrl + this.brokerdataUrl)
      .subscribe(res => {
        if (res.status === '0') {
          this.imBroker = res.imList;
          this.exBroker = res.exList;
        }
        this.getData();
      }, error => {
        console.log(error);
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
        if (temp.controls.shippercompany === undefined) {
          temp.addControl('shippercompany', new FormControl(''));
        }
        if (temp.controls.shipper === undefined) {
          temp.addControl('shipper', new FormControl(''));
        }
        if (temp.controls.receivercompany === undefined) {
          temp.addControl('receivercompany', new FormControl(''));
        }
        if (temp.controls.receiver === undefined) {
          temp.addControl('receiver', new FormControl(''));
        }
        if (temp.controls.receiverzipcode === undefined) {
          temp.addControl('receiverzipcode', new FormControl(''));
        }
        if (temp.controls.receiveraddr === undefined) {
          temp.addControl('receiveraddr', new FormControl(''));
        }
        if (temp.controls.receiverphone === undefined) {
          temp.addControl('receiverphone', new FormControl(''));
        }
        if (temp.controls.receivertaxid === undefined) {
          temp.addControl('receivertaxid', new FormControl(''));
        }
      }
      control.patchValue(this.headerData);
    } else {
      const control = this.dataForm.get('boxform') as FormArray;
      for (let i = 0; i < control.length; i++) {
        const temp = control.controls[i] as FormGroup;
        if (temp.controls.shippercompany !== undefined) {
          temp.removeControl('shippercompany');
        }
        if (temp.controls.shipper !== undefined) {
          temp.removeControl('shipper');
        }
        if (temp.controls.receivercompany !== undefined) {
          temp.removeControl('receivercompany');
        }
        if (temp.controls.receiver !== undefined) {
          temp.removeControl('receiver');
        }
        if (temp.controls.receiverzipcode !== undefined) {
          temp.removeControl('receiverzipcode');
        }
        if (temp.controls.receiveraddr !== undefined) {
          temp.removeControl('receiveraddr');
        }
        if (temp.controls.receiverphone !== undefined) {
          temp.removeControl('receiverphone');
        }
        if (temp.controls.receivertaxid !== undefined) {
          temp.removeControl('receivertaxid');
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
        clearanceno: ['', [Validators.required]],
        transferno: ['', [Validators.required]],
        weight: [''],
        totalitem: [''],
        logistics: [''],
        shipperremark: [''],
        productform: this.formBuilder.array([
          this.initProduct()
        ]),
        decform: this.formBuilder.array([
          this.initDec()
        ]),
        shippercompany: [''],
        shipper: [''],
        receivercompany: [''],
        receiver: [''],
        receiverzipcode: [''],
        receiveraddr: [''],
        receiverphone: [''],
        receivertaxid: [''],
      });
    } else {
      return this.formBuilder.group({
        id: [0],
        clearanceno: ['', [Validators.required]],
        transferno: ['', [Validators.required]],
        weight: [''],
        totalitem: [''],
        logistics: [''],
        shipperremark: [''],
        productform: this.formBuilder.array([
          this.initProduct()
        ]),
        decform: this.formBuilder.array([
          this.initDec()
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
      zipcode: [''],
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

  adddec(ibox) {
    const control = (this.dataForm.controls.boxform as FormArray).at(ibox).get('decform') as FormArray;
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

  removedec(rowIndex, subrowIndex) {
    const rows = (this.dataForm.controls.boxform as FormArray).at(rowIndex).get('decform') as FormArray;
    const row = rows.controls[subrowIndex] as FormGroup;
    if (row.controls.id.value !== 0) {
      this.deciddellist.push(row.controls.id.value);
    }
    rows.removeAt(subrowIndex);

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
            clearanceno: [this.headerData[i].clearanceno],
            transferno: [this.headerData[i].transferno],
            weight: [this.headerData[i].weight],
            totalitem: [this.headerData[i].totalitem],
            logistics: [this.headerData[i].logistics],
            shipperremark: [this.headerData[i].shipperremark],
            productform: this.formBuilder.array([
            ]),
            decform: this.formBuilder.array([
            ]),
            shippercompany: [this.headerData[i].shippercompany],
            shipper: [this.headerData[i].shipper],
            receivercompany: [this.headerData[i].receivercompany],
            receiver: [this.headerData[i].receiver],
            receiverzipcode: [this.headerData[i].receiverzipcode],
            receiveraddr: [this.headerData[i].receiveraddr],
            receiverphone: [this.headerData[i].receiverphone],
            receivertaxid: [this.headerData[i].receivertaxid]
          }));
        } else {
          control.push(this.formBuilder.group({
            id: [this.headerData[i].id],
            clearanceno: [this.headerData[i].clearanceno],
            transferno: [this.headerData[i].transferno],
            weight: [this.headerData[i].weight],
            totalitem: [this.headerData[i].totalitem],
            logistics: [this.headerData[i].logistics],
            shipperremark: [this.headerData[i].shipperremark],
            productform: this.formBuilder.array([
            ]),
            decform: this.formBuilder.array([
            ])
          }));
        }
      }

      for (let j = 0; j < this.headerData.length; j++) {
        const control2 = control.at(j).get('productform') as FormArray;
        const control3 = control.at(j).get('decform') as FormArray;
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

        // tslint:disable-next-line: prefer-for-of
        for (let k = 0; k < this.declarData.length; k++) {
          if (this.headerData[j].id === this.declarData[k].shippingiD_H) {
            control3.push(this.formBuilder.group({
              id: [this.declarData[k].id],
              name: [this.declarData[k].name, Validators.required],
              taxid: [this.declarData[k].taxid, Validators.required],
              phone: [this.declarData[k].phone],
              mobile: [this.declarData[k].mobile],
              zipcode: [this.declarData[k].zipcode],
              addr: [this.declarData[k].addr],
              idphotof: [this.declarData[k].idphotof],
              idphotob: [this.declarData[k].idphotob],
              appointment: [this.declarData[k].appointment]
            }));
          }
        }

      }
    }

  }

  //#endregion

  saveData(form) {
    // console.log(form);
    // check Form
    if (this.dataForm.invalid) {
      return alert('必填不能為空！');
    }

    const postData = { delH: this.hiddellist, delD: this.diddellist, delDec: this.deciddellist, formdata: form };
    this.commonService.editShippingData(postData, this.baseUrl + this.dataEditUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.getEditData();
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
