import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { MenuInfo } from 'src/app/_Helper/models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-menu-list',
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.css']
})
export class MenuListComponent implements OnInit {
  /* Web api url*/
  private baseUrl = window.location.origin + '/api/SysHelp/';
  private visibleUrl = 'EditTSMenuVisibleData';
  private enableUrl = 'EditTSMenuEnableData';
  private menuDataUrl = 'GetMenuData';
  private menuEditUrl = 'EditMenuData';

  tableTitle = ['名稱', '代碼', 'URL', '敘述', '建立時間',
    '建立者', '更新時間', '更新者', '列表顯示', '停用', '編輯'];
  Backdata: MenuInfo[];
  Frontdata: MenuInfo[];
  dataForm: FormGroup;
  parentData: MenuInfo[];
  menuData: MenuInfo;
  dataChange;

  visibleList: any = [];
  activeList: any = [];
  activeback = true;
  isErr = false;
  chkNum = 1;
  modalid: string;

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
    this.commonService.getBacknFrontMenu()
      .subscribe(
        data => {
          if (data.status === '0') {
            this.Backdata = data.backList;
            this.Frontdata = data.frontList;
          } else {
            this.Backdata = null;
            this.Frontdata = null;
          }
        },
        error => {
          console.log(error);
        });
  }

  tabSwitch(id) {
    if (id === 'tab1') {
      this.activeback = true;
    } else {
      this.activeback = false;
    }
  }

  doIsvisible() {
    if (this.visibleList.length > 0) {
      this.commonService.editEnableData(this.visibleList, this.baseUrl + this.visibleUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.getData();
            this.visibleList = [];
          } else {
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });

    } else { alert('頁面無項目變更！'); }
  }

  visibleSelChange(val, Ischk) {
    this.chkNum = 0;
    this.visibleList.map((item) => {
      if (item.id === val) {
        item.isenable = Ischk;
        this.chkNum++;
      }
    });

    if (this.visibleList.length === 0) {
      this.visibleList.push({ id: val, isenable: Ischk });
    } else {
      if (this.chkNum === 0) {
        this.visibleList.push({ id: val, isenable: Ischk });
      }
    }

  }

  doIsenable() {
    if (this.activeList.length > 0) {
      this.commonService.editEnableData(this.activeList, this.baseUrl + this.enableUrl)
        .subscribe(data => {
          if (data.status === '0') {
            alert(data.msg);
            this.getData();
            this.activeList = [];
          } else {
            alert(data.msg);
          }
        },
          error => {
            console.log(error);
          });

    } else { alert('頁面無項目變更！'); }
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
  openModal(id: string, menuid) {

    this.modalid = id;
    // get menuParentData
    this.parentData = null;
    this.commonService.getParentMenu(this.activeback ? '1' : '0')
      .subscribe(data => {
        this.parentData = data.pList;
      },
        error => {
          console.log(error);
        });

    // get menuData
    if (menuid !== 0) {
      this.commonService.getSingleData(menuid, this.baseUrl + this.menuDataUrl)
        .subscribe(data => {
          this.dataForm.patchValue(data.rows);
          this.dataForm.controls.menucode.disable();
          this.menuData = data.rows;
        },
          error => {
            console.log(error);
          });
    }

    this.modalService.open(id);
  }

  closeModal(id: string) {
    this.isErr = false;
    this.dataForm.controls.menucode.enable();
    this.resetForm();
    this.modalService.close(id);
  }

  saveData(form) {
    // check Form
    if (this.dataForm.invalid) {
      this.isErr = true;
      return alert('代碼不能為空！');
    }

    form.isback = this.activeback ? '1' : '0';

    if (form.menuid !== 0) {
      this.dataChange = this.commonService.formChanges(form, this.menuData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: form.menuid, formdata: this.dataChange };
      this.commonService.editSingleData(postData, this.baseUrl + this.menuEditUrl)
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

  resetForm() {
    this.dataForm = this.formBuilder.group({
      menuid: 0,
      parentcode: '0',
      menucode: ['', Validators.required],
      menuname: '',
      menudesc: '',
      menuseq: '0',
      menuurl: '',
      iconurl: '',
      isback: '0',
      isvisible: '0',
      isenable: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: ''
    });
  }

}
