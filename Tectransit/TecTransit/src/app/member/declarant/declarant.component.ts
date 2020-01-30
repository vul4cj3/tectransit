import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';
import { DeclarantInfo } from 'src/app/_Helper/models';

class ImgSnippet {
  constructor(public src: string, public file: File) { }
}

@Component({
  selector: 'app-declarant',
  templateUrl: './declarant.component.html',
  styleUrls: ['./declarant.component.css']
})
export class DeclarantComponent implements OnInit {

  dataForm: FormGroup;

  private commUrl = window.location.origin + '/api/CommonHelp/';
  private FileUploadUrl = 'UploadFileData';
  getdataUrl = '/api/Member/GetACDeclarantData';
  getdata2Url = '/api/Member/GetDeclarantData';
  saveUrl = '/api/Member/SaveDeclarantData';

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
      idphoto_f: [''],
      idphoto_b: [''],
      appointment: ['']
    });
  }

  getData() {
    this.commonservice.getSingleData(this.getdataUrl)
      .subscribe(data => {
        if (data.rows.length > 0) {
          this.dataList = data.rows;
        }
      },
        error => {
          console.log(error);
        });
  }

  editData() {

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

    // const file: File = imgInput.files[0];
    // const reader = new FileReader();

    /*
    reader.addEventListener('load', (event: any) => {
      this.selectedFile = new ImgSnippet(event.target.result, file);
      this.commonservice.imageUpload('dec', this.selectedFile.file)
      .subscribe(data => {
        if (data.status === '0') {
          this.dataForm.controls.idphoto_f.setValue(data.imgurl);
        }
      },
        error => {
          console.log(error);
        });
    });*/

    // reader.readAsDataURL(file);

  }

  fileChange(e) {
    this.fileList = [];
    this.fileList.push(e.target.files[0]);
  }

  saveData(form) {
    // 先上傳檔案
    const promise = new Promise((resolve, reject) => {
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
            this.dataForm.controls.idphoto_f.setValue(temp[0]);
            this.dataForm.controls.idphoto_b.setValue(temp[1]);

            resolve('success');
          }
        },
          error => {
            console.log(error);
          });
    });

    const promise1 = new Promise((resolve, reject) => {
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
          alert(data.msg);
        }
      },
        error => {
          console.log(error);
        });
  }

}
