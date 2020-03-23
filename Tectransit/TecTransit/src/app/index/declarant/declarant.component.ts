import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';
import { DeclarantInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-declarant',
  templateUrl: './declarant.component.html',
  styleUrls: ['./declarant.component.css']
})

export class DeclarantComponent implements OnInit {

  dataForm: FormGroup;

  getdataUrl = '/api/Member/GetACDeclarantData';
  getdata2Url = '/api/Member/GetDeclarantData';
  deldataUrl = '/api/Member/EditDeclarantData';
  saveUrl = '/api/Member/SaveDeclarantData';
  testUrl = '/api/DepotCheck/';

  dataList: DeclarantInfo[];
  dataChange;
  idList;

  selectedFile: string[];
  imgList: string[];
  fileList: string[];

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
      id: [0],
      name: ['', Validators.required],
      taxid: [''],
      phone: [''],
      mobile: [''],
      addr: ['', Validators.required],
      idphotO_F: [''],
      idphotO_B: [''],
      appointment: ['']
    });

    const div = document.getElementById('img-preview') as HTMLDivElement;
    div.innerHTML = '';
  }

  getData() {
    this.commonservice.getSingleData(this.getdataUrl)
      .subscribe(data => {
        if (data.rows.length > 0) {
          this.dataList = data.rows;
        } else {
          this.dataList = null;
        }
      },
        error => {
          console.log(error);
        });
  }

  editData(id) {
    this.commonservice.getData(id, this.getdata2Url)
      .subscribe(data => {
        this.dataForm.patchValue(data.rows);
        console.log(data.rows);
        console.log(this.dataForm);
      },
        error => {
          console.log(error);
        });
  }

  delData(id) {
    this.commonservice.delSingleData(id, this.deldataUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.getData();
          alert(data.msg);
        }
      },
        error => {
          console.log(error);
        });
  }

  imgChange(e) {
    if (e.target.files.length > 2) {
      return alert('請勿上傳超過兩個檔案！');
    }

    this.imgList = [];

    const div = document.getElementById('img-preview') as HTMLDivElement;
    div.innerHTML = '';
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < e.target.files.length; i++) {
      this.imgList.push(e.target.files[i]);

      const file = e.target.files[i];
      const reader = new FileReader();
      reader.addEventListener('load', (event: any) => {
        div.innerHTML += '<img class=\'thumb-nail\' src=\'' + event.target.result + '\'' + 'title=\'' + file.name + '\'/>';
      });
      reader.readAsDataURL(file);
    }
  }

  fileChange(e) {
    this.fileList = [];
    this.fileList.push(e.target.files[0]);
  }

  saveData(form) {
    // 先上傳檔案
    const promise = new Promise((resolve, reject) => {
      if (this.imgList !== undefined) {
        const formdata = new FormData();
        for (let i = 0; i < this.imgList.length; i++) {
          formdata.append('fileUpload', this.imgList[i]);
          if (i === 0) {
            formdata.append('TYPE', 'dec');
          }
        }

        this.commonservice.imageUpload(formdata)
          .subscribe(data => {
            if (data.status === '0') {
              const temp = data.imgurl.split(';');
              this.dataForm.controls.idphotO_F.setValue(temp[0]);
              this.dataForm.controls.idphotO_B.setValue(temp[1]);

              resolve('success');
            }
          },
            error => {
              console.log(error);
            });
      } else { resolve('success'); }
    });

    const promise1 = new Promise((resolve, reject) => {
      if (this.fileList !== undefined) {
        const formdata = new FormData();
        for (let i = 0; i < this.fileList.length; i++) {
          formdata.append('fileUpload', this.fileList[i]);
          if (i === 0) {
            formdata.append('TYPE', 'dec');
          }
        }

        this.commonservice.fileUpload(formdata)
          .subscribe(data => {
            if (data.status === '0') {
              this.dataForm.controls.appointment.setValue(data.fileurl);

              resolve('success');
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

  doCancel() {
    this.resetForm();
  }

  save() {
    this.commonservice.editData(this.dataForm.value, this.saveUrl)
      .subscribe(data => {
        if (data.status === '0') {
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
