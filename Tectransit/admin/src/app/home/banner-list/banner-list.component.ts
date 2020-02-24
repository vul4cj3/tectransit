import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, RequiredValidator } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { ModalService } from 'src/app/services/modal.service';
import { BannerInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-banner-list',
  templateUrl: './banner-list.component.html',
  styleUrls: ['./banner-list.component.css']
})
export class BannerListComponent implements OnInit {

  /* Web api url*/
  private commUrl = window.location.origin + '/api/CommonHelp/';
  private baseUrl = window.location.origin + '/api/WebSetHelp/';
  private banDataUrl = 'GetBanData';
  private BanEditUrl = 'EditTDBanData';
  private topUrl = 'EditTDBanTopData';
  private enableUrl = 'EditTDBanEnableData';
  private delbanUrl = 'DelTDBanData';
  private banUploadUrl = 'UploadImgData';

  tableTitle = ['#', '標題', '簡述', '圖片Url', 'Url', '上架日期(起訖)', '置頂', '停用', '編輯'];
  data: BannerInfo[];
  dataForm: FormGroup;
  dataChange;
  BanData: BannerInfo;

  chkList: any = [];
  topList: any = [];
  activeList: any = [];
  isErr = false;
  chkNum = 1;
  modalid: string;

  tempList: any = [];
  imgList: any = [];
  previewUrl: any = null;
  fileUploadProgress: string = null;
  uploadedFilePath: string = null;

  constructor(
    private formBuilder: FormBuilder,
    public commonService: CommonService,
    private modalService: ModalService
  ) { }

  ngOnInit() {
    // built form controls and default form value
    this.resetForm();
    this.getData();
  }

  getData() {
    // get menu
    this.commonService.getAllBanner()
      .subscribe(
        data => {
          if (data.status === '0') {
            this.data = data.dataList;
          } else {
            this.data = null;
          }
        },
        error => {
          console.log(error);
        });
  }

  doIstop() {
    if (this.topList.length > 0) {
      this.commonService.editTopData(this.topList, this.baseUrl + this.topUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.getData();
            this.topList = [];
            this.activeList = [];
            this.chkList = [];
          } else {
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });

    } else { alert('頁面無項目變更！'); }
  }

  doIsenable() {
    if (this.activeList.length > 0) {
      this.commonService.editEnableData(this.activeList, this.baseUrl + this.enableUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.getData();
            this.topList = [];
            this.activeList = [];
            this.chkList = [];
          } else {
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });

    } else { alert('頁面無項目變更！'); }
  }

  doDelete() {
    const IsConfirm = confirm('確定要刪除？');
    if (IsConfirm === true) {
      if (this.chkList.length > 0) {
        this.commonService.delData(this.chkList, this.baseUrl + this.delbanUrl)
          .subscribe(data => {
            if (data.status === '0') {
              alert(data.msg);
              this.getData();
              this.topList = [];
              this.activeList = [];
              this.chkList = [];
            } else {
              alert(data.msg);
            }
          },
            error => {
              console.log(error);
            });

      } else { alert('頁面無項目變更！'); }
    } else { }
  }

  SelChange(val, Ischk) {
    this.chkNum = 0;
    this.chkList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.chkList.length === 0) {
      this.chkList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.chkList.push({ id: val, isenable: Ischk });
      }
    }

  }

  topSelChange(val, Ischk) {
    this.chkNum = 0;
    this.topList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.topList.length === 0) {
      this.topList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.topList.push({ id: val, isenable: Ischk });
      }
    }

  }

  enableSelChange(val, Ischk) {
    this.chkNum = 0;
    this.activeList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.activeList.length === 0) {
      this.activeList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.activeList.push({ id: val, isenable: Ischk });
      }
    }

  }



  /* Popup window function */
  openModal(id: string, banid) {
    this.modalid = id;
    // get menuData
    if (banid !== 0) {
      this.commonService.getSingleData(banid, this.baseUrl + this.banDataUrl)
        .subscribe(data => {
          this.dataForm.patchValue(data.rows);
          this.BanData = data.rows;
        },
          error => {
            console.log(error);
          });
    } else { this.resetForm(); }

    this.modalService.open(id);
  }

  closeModal(id: string) {
    this.isErr = false;
    this.resetForm();
    this.modalService.close(id);
  }

  saveData(form) {

    // check Form
    if (this.dataForm.invalid) {
      this.isErr = true;
      return alert('代碼不能為空！');
    }

    // 先上傳檔案
    const promise = new Promise((resolve, reject) => {
      if (this.imgList.length > 0) {
        const formdata = new FormData();
        formdata.append('fileUpload', this.imgList[0].file);
        formdata.append('TYPE', 'ban');

        this.commonService.fileUpload(formdata, this.commUrl + this.banUploadUrl)
          .subscribe(data => {
            if (data.status === '0') {
              this.dataForm.controls.imgurl.setValue(data.imgurl);

              resolve('success');
            }
          },
            error => {
              console.log(error);
            });
      } else { resolve('success'); }
    });

    Promise.all([promise])
      .then(() => {
        this.save(this.dataForm.value);
      });

  }

  save(form) {
    if (form.banid !== 0) {
      this.dataChange = this.commonService.formChanges(form, this.BanData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: form.banid, formdata: this.dataChange };
      this.commonService.editSingleData(postData, this.baseUrl + this.BanEditUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.getData();
            this.closeModal(this.modalid);
          } else {
            this.isErr = true;
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });
    } else { alert('頁面無數據被修改！'); }
  }

  fileChange(e) {

    this.imgList = [];

    const div = document.getElementById('img-preview') as HTMLDivElement;
    div.innerHTML = '';

    this.imgList.push({ file: e.target.files[0] });

    const file = e.target.files[0];
    const reader = new FileReader();
    reader.addEventListener('load', (event: any) => {
      div.innerHTML += '<img class=\'thumb-nail\' src=\'' + event.target.result + '\'' + 'title=\'' + file.name + '\'/>';
    });
    reader.readAsDataURL(file);

  }

  resetForm() {
    const div = document.getElementById('img-preview') as HTMLDivElement;
    div.innerHTML = '';

    this.dataForm = this.formBuilder.group({
      banid: 0,
      title: ['', Validators.required],
      descr: '',
      imgurl: '',
      url: '',
      banseq: '0',
      upsdate: '',
      upedate: '',
      istop: '0',
      isenable: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: ''
    });
  }

}
