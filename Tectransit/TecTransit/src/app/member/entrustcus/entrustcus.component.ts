import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl, NgControlStatus } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';
import { IDImgList, IDFileList } from 'src/app/_Helper/models';

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

  tempList: any = [];
  imgList: any = [];
  fileList: any = [];

  idimglist: IDImgList[];
  idfilelist: IDFileList[];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    // 檢查是否為廠商會員,非廠商會員不可進入
    this.chkMem();

    this.resetForm();
    this.getData();
  }

  chkMem() {
    this.commonservice.chkMemtype()
      .subscribe(data => {
        if (data.data === 'N') {
          this.router.navigate(['/']);
        }
      },
        error => {
          console.log(error);
        });
  }

  resetForm() {
    const chk = document.getElementById('IsmultReceiver') as HTMLInputElement;
    chk.checked = false;
    this.IsmultRec = false;

    this.dataForm = this.formBuilder.group({
      stationcode: [''],
      trasferno: ['', Validators.required],
      total: ['', Validators.required],
      idphotof: [''],
      idphotob: [''],
      appointment: [''],
      receiver: [''],
      receiveraddr: [''],
      ismultreceiver: [''],
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
        receiver: [''],
        receiveraddr: ['']
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
      product: ['', Validators.required],
      quantity: ['', Validators.required],
      unitprice: ['', Validators.required],
      // ---------------------------------------------------------------------
    });
  }

  initDec() {
    return this.formBuilder.group({
      //  ---------------------forms fields ------------------------
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

  addrec() {
    if (this.IsmultRec) {
      const control = this.dataForm.get('boxform') as FormArray;
      for (let i = 0; i < control.length; i++) {
        const temp = control.controls[i] as FormGroup;
        temp.addControl('receiver', new FormControl(''));
        temp.addControl('receiveraddr', new FormControl(''));
      }
    } else {
      this.dataForm.addControl('receiver', new FormControl(''));
      this.dataForm.addControl('receiveraddr', new FormControl(''));
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

    this.refreshImg(rowIndex);
    this.refreshFile(rowIndex);
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

  //#region upload function
  imgChange(e, id) {
    if (e.target.files.length > 2) {
      return alert('請勿上傳超過兩個檔案！');
    }

    this.tempList = [];
    for (const item of this.imgList) {
      if (item.idcode !== id) {
        this.tempList.push(item);
      }
    }
    this.imgList = this.tempList;

    const div = document.getElementById('img-preview' + id) as HTMLDivElement;
    div.innerHTML = '';
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < e.target.files.length; i++) {
      this.imgList.push({ idcode: id, file: e.target.files[i] });

      const file = e.target.files[i];
      const reader = new FileReader();
      reader.addEventListener('load', (event: any) => {
        div.innerHTML += '<img class=\'thumb-nail\' src=\'' + event.target.result + '\'' + 'title=\'' + file.name + '\'/>';
      });
      reader.readAsDataURL(file);
    }
  }

  refreshImg(id) {
    this.tempList = [];
    for (const item of this.imgList) {
      if (item.idcode !== id) {
        this.tempList.push(item);
      }
    }
    this.imgList = this.tempList;
  }

  fileChange(e, id) {
    this.tempList = [];
    for (const item of this.fileList) {
      if (item.idcode !== id) {
        this.tempList.push(item);
      }
    }
    this.fileList = this.tempList;

    this.fileList.push({ idcode: id, file: e.target.files[0] });

    console.log(this.fileList);
  }

  refreshFile(id) {
    this.tempList = [];
    for (const item of this.fileList) {
      if (item.idcode !== id) {
        this.tempList.push(item);
      }
    }
    this.fileList = this.tempList;
  }
  //#endregion

  getData() {
    this.commonservice.getSingleData(this.getStationUrl)
      .subscribe(data => {
        this.stationData = data.rows;
      }, error => {
        console.log(error);
      });
  }

  saveData(form) {
    if (form.stationcode === '') {
      return alert('必須選擇集運站！');
    }

    if (this.dataForm.invalid) {
      return alert('必填欄位不能為空白！');
    }

    // 先上傳檔案
    const promise = new Promise((resolve, reject) => {
      if (this.imgList.length > 0) {
        const formdata = new FormData();
        for (let i = 0; i < this.imgList.length; i++) {
          formdata.append('idcode', this.imgList[i].idcode);
          formdata.append('fileUpload', this.imgList[i].file);
          if (i === 0) {
            formdata.append('TYPE', 'shipping_cus');
          }
        }

        this.commonservice.imageUpload(formdata)
          .subscribe(data => {
            if (data.status === '0') {
              this.idimglist = data.imgurl;
              if (this.idimglist.length > 0) {
                for (const item of this.idimglist) {
                  const control = (this.dataForm.get('decform') as FormArray).at(+item.id) as FormGroup;
                  control.controls.idphotof.setValue(item.idphotof);
                  control.controls.idphotob.setValue(item.idphotob);
                }
              }

              resolve('success');
            }
          },
            error => {
              console.log(error);
            });
      } else { resolve('success'); }
    });

    const promise1 = new Promise((resolve, reject) => {
      if (this.fileList.length > 0) {
        const formdata2 = new FormData();
        for (let i = 0; i < this.fileList.length; i++) {
          formdata2.append('idcode', this.fileList[i].idcode);
          formdata2.append('fileUpload', this.fileList[i].file);
          if (i === 0) {
            formdata2.append('TYPE', 'shipping_cus');
          }
        }

        this.commonservice.fileUpload(formdata2)
          .subscribe(data => {
            if (data.status === '0') {
              this.idfilelist = data.fileurl;
              if (this.idfilelist.length > 0) {
                for (const item of this.idfilelist) {
                  const control = (this.dataForm.get('decform') as FormArray).at(+item.id) as FormGroup;
                  control.controls.appointment.setValue(item.appointment);
                }

                resolve('success');
              }
            }
          },
            error => {
              console.log(error);
            });
      } else { resolve('success'); }
    });

    Promise.all([promise, promise1])
      .then(() => {
        this.save();
      });

  }

  save() {
    this.commonservice.editData(this.dataForm.value, this.saveUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.resetForm();
          alert(data.msg);
        }
      },
        error => {
          console.log(error);
        });
  }
}
