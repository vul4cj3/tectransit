import { Component, OnInit, ɵConsole } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute } from '@angular/router';
import { ShippingMCusInfo, ShippingHCusInfo, ShippingDCusInfo, DeclarantCusInfo, IDImgList, IDFileList } from 'src/app/_Helper/models';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';

@Component({
  selector: 'app-shippingcus-edit',
  templateUrl: './shippingcus-edit.component.html',
  styleUrls: ['./shippingcus-edit.component.css']
})
export class ShippingcusEditComponent implements OnInit {

  /* Web api url*/
  private baseUrl = '/api/Member/';
  private dataUrl = 'GetSingleShippingCusData';
  private saveUrl = 'SaveCusShippingDecData';

  dataForm: FormGroup;

  shippingType;
  dataID;

  isExist = false;
  isModify = false;
  masterData: ShippingMCusInfo;
  headerData: ShippingHCusInfo[];
  detailData: ShippingDCusInfo[];
  declarData: DeclarantCusInfo[];

  tempList: any = [];
  imgList: any = [];
  fileList: any = [];
  iddellist: any = [];

  idimglist: IDImgList[];
  idfilelist: IDFileList[];

  constructor(
    private formBuilder: FormBuilder,
    public commonservice: CommonService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.shippingType = this.route.snapshot.paramMap.get('type');
    this.dataID = this.route.snapshot.paramMap.get('id');

    // declarant data can modify or not
    this.isExist = (this.shippingType === 't5' ? false : true);

    this.resetForm();
    this.getData();
  }

  resetForm() {
    this.dataForm = this.formBuilder.group({
      // shippingidm: [''],
      // shippingidh: [''],
      boxform: this.formBuilder.array([
        this.initBox()
      ])
      // decform: this.formBuilder.array([
      //   this.initDec()
      // ])
    });
  }

  initBox() {
    return this.formBuilder.group({
      shippingidm: [''],
      shippingidh: [''],
      decform: this.formBuilder.array([
        this.initDec()
      ])
    });
  }

  readBox(id) {
    const control = this.dataForm.controls.boxform as FormArray;
    control.clear();
    if (this.headerData.length > 0) {
      // tslint:disable-next-line: prefer-for-of
      for (let i = 0; i < this.headerData.length; i++) {
        if (this.headerData[i].id === id) {
          control.push(this.formBuilder.group({
            shippingidm: [this.headerData[i].shippingiD_M],
            shippingidh: [this.headerData[i].id],
            decform: this.formBuilder.array([
            ])
          }));
        }
      }


      const control2 = control.at(0).get('decform') as FormArray;
      control2.clear();
      // tslint:disable-next-line: prefer-for-of
      for (let k = 0; k < this.declarData.length; k++) {
        if (this.declarData[k].shippingiD_H === id) {
          control2.push(this.formBuilder.group({
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

  adddec() {
    const control = (this.dataForm.controls.boxform as FormArray).at(0).get('decform') as FormArray;
    control.push(this.initDec());
  }

  removedec(rowIndex) {
    const rows = (this.dataForm.controls.boxform as FormArray).at(0).get('decform') as FormArray;
    const row = rows.controls[rowIndex] as FormGroup;
    if (row.controls.id.value !== 0) {
      this.iddellist.push(row.controls.id.value);
    }
    rows.removeAt(rowIndex);

    this.refreshImg(rowIndex);
    this.refreshFile(rowIndex);
  }

  getData() {
    this.commonservice.getData(this.dataID, this.baseUrl + this.dataUrl)
      .subscribe(
        data => {
          if (data.status === '0') {
            this.masterData = data.rowM;
            this.headerData = data.rowH;
            this.detailData = data.rowD;
            this.declarData = data.rowDec;

          }
        },
        error => {
          console.log(error);
        });
  }

  chgDec(type, hid) {
    if (type === 'm') {
      this.readBox(hid);

      // 檢查有無其他修改中的表格
      this.clearTable();

      const btn = document.getElementById('btn' + hid) as HTMLDivElement;
      btn.children.item(0).classList.add('inActive');
      btn.children.item(1).classList.add('Active');
      btn.children.item(2).classList.add('Active');

      const decform = document.getElementById('decform' + hid) as HTMLTableElement;
      decform.classList.add('Active');

      const dectb = document.getElementById('dectable' + hid) as HTMLTableElement;
      dectb.classList.add('inActive');

    } else {
      this.iddellist = [];
      this.resetForm();

      const btn = document.getElementById('btn' + hid) as HTMLDivElement;
      btn.children.item(0).classList.remove('inActive');
      btn.children.item(1).classList.remove('Active');
      btn.children.item(2).classList.remove('Active');

      const decform = document.getElementById('decform' + hid) as HTMLTableElement;
      decform.classList.remove('Active');

      const dectb = document.getElementById('dectable' + hid) as HTMLTableElement;
      dectb.classList.remove('inActive');
    }
    // if (type === 'm') {
    //   this.isModify = true;

    //   // read declarant data
    //   this.dataForm.controls.shippingidm.setValue(this.masterData.id);
    //   const control = this.dataForm.controls.decform as FormArray;
    //   if (this.declarData.length > 0) {
    //     for (let i = 0; i < this.declarData.length; i++) {
    //       if (control.at(i) !== undefined) {
    //         console.log(this.declarData[i].shippingiD_H);
    //         if (this.declarData[i].shippingiD_H === hid) {
    //           control.at(i).patchValue(this.declarData[i]);
    //         }
    //       } else {
    //         control.push(this.formBuilder.group({
    //           id: [this.declarData[i].id],
    //           name: [this.declarData[i].name, Validators.required],
    //           taxid: [this.declarData[i].taxid, Validators.required],
    //           phone: [this.declarData[i].phone],
    //           mobile: [this.declarData[i].mobile],
    //           addr: [this.declarData[i].addr],
    //           idphotof: [this.declarData[i].idphotof],
    //           idphotob: [this.declarData[i].idphotob],
    //           appointment: [this.declarData[i].appointment]
    //         }));
    //       }

    //     }
    //   }
    // } else {
    //   this.isModify = false;
    //   this.iddellist = [];
    //   this.resetForm();
    // }
  }

  clearTable() {

    const btnAll = document.getElementsByClassName('btn-three');
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < btnAll.length; i++) {
      const item = btnAll[i] as HTMLDivElement;
      item.children.item(0).classList.remove('inActive');
      item.children.item(1).classList.remove('Active');
      item.children.item(2).classList.remove('Active');
    }

    const decformAll = document.getElementsByClassName('decform-modify');
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < decformAll.length; i++) {
      const item = decformAll[i] as HTMLTableElement;
      item.classList.remove('Active');
    }

    const dectbAll = document.getElementsByClassName('declarant-table');
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < dectbAll.length; i++) {
      const item = dectbAll[i] as HTMLTableElement;
      item.classList.remove('inActive');
    }
  }

  //#region upload function
  imgChange(e, hid, id) {
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

    const div = document.getElementById('img-preview' + hid + '_' + id) as HTMLDivElement;
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

  fileChange(e, hid, id) {
    this.tempList = [];
    for (const item of this.fileList) {
      if (item.idcode !== id) {
        this.tempList.push(item);
      }
    }
    this.fileList = this.tempList;

    const div = document.getElementById('file-preview-container' + hid + '_' + id) as HTMLDivElement;
    div.innerHTML = '<span>' + e.target.files[0].name + '</span>';

    this.fileList.push({ idcode: id, file: e.target.files[0] });
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

  saveData(form) {
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
                  const box = (this.dataForm.controls.boxform as FormArray).at(0).get('decform') as FormArray;
                  const control = box.at(+item.id) as FormGroup;
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
                  const box = (this.dataForm.controls.boxform as FormArray).at(0).get('decform') as FormArray;
                  const control = box.at(+item.id) as FormGroup;
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
    this.commonservice.editNDelData(this.dataForm.value, this.iddellist, this.baseUrl + this.saveUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.iddellist = [];
          this.resetForm();
          this.getData();
          alert(data.msg);
        }
      },
        error => {
          console.log(error);
        });
  }
}
