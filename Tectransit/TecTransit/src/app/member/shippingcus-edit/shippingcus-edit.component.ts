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
  cateID;
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
    this.cateID = this.route.snapshot.paramMap.get('code');
    this.dataID = this.route.snapshot.paramMap.get('id');

    // declarant data can modify or not
    this.isExist = (this.shippingType === 't5' ? false : true);

    this.resetForm();
    this.getData();
  }

  resetForm() {
    this.dataForm = this.formBuilder.group({
      shippingidm: [''],
      decform: this.formBuilder.array([
        this.initDec()
      ])
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

  adddec() {
    const control = this.dataForm.controls.decform as FormArray;
    control.push(this.initDec());
  }

  removedec(rowIndex) {
    const rows = this.dataForm.controls.decform as FormArray;
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

  chgDec(type) {
    if (type === 'm') {
      this.isModify = true;

      // read declarant data
      this.dataForm.controls.shippingidm.setValue(this.masterData.id);
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
    } else {
      this.isModify = false;
      this.iddellist = [];
      this.resetForm();
    }
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

    const div = document.getElementById('file-preview-container' + id) as HTMLDivElement;
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
    this.commonservice.editNDelData(this.dataForm.value, this.iddellist, this.baseUrl + this.saveUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.isModify = false;
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
