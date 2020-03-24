import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountInfo } from 'src/app/_Helper/models';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-profilecus',
  templateUrl: './profilecus.component.html',
  styleUrls: ['./profilecus.component.css']
})
export class ProfilecusComponent implements OnInit {
  memForm: FormGroup;

  getdataUrl = '/api/Member/GetCusMemData';
  saveUrl = '/api/Member/EditCusMemData';

  memData: AccountInfo;
  dataChange;

  isShow = false;
  isShow2 = false;
  isShow3 = false;

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
    this.memForm = this.formBuilder.group({
      userid: [''],
      usercode: ['', Validators.required],
      userpassword: [''],
      newpw: [''],
      companyname: [''],
      rateid: [''],
      username: ['', Validators.required],
      email: ['', Validators.required],
      address: ['', Validators.required],
      phone: ['', Validators.required],
      mobile: ['', Validators.required]
    });
  }

  getData() {
    this.commonservice.getSingleData(this.getdataUrl)
      .subscribe(data => {
        this.memForm.patchValue(data.rows);
        this.memData = data.rows;
      }, error => {
        console.log(error);
      });
  }

  showPW(id) {
    const controls = document.getElementById(id) as HTMLInputElement;
    if (controls.type === 'password') {
      if (id === 'userpassword') {
        this.isShow = true;
      } else if (id === 'newpw') {
        this.isShow2 = true;
      } else {
        this.isShow3 = true;
      }
      controls.type = 'text';
    } else {
      if (id === 'userpassword') {
        this.isShow = false;
      } else if (id === 'newpw') {
        this.isShow2 = false;
      } else {
        this.isShow3 = false;
      }
      controls.type = 'password';
    }
  }

  SaveData(form) {

    // stop here if form is invalid
    if (this.memForm.invalid) {
      return alert('欄位不能為空白！');
    }

    /* 密碼變更欄位檢查 */
    const oldpw = (document.getElementById('userpassword')) as HTMLInputElement;
    const npw = (document.getElementById('newpw')) as HTMLInputElement;
    const cpw = (document.getElementById('confirmpw')) as HTMLInputElement;
    if (oldpw.value !== '') {
      if (npw.value === '') {
        return alert('請輸入新密碼值！');
      } else if (cpw.value === '') {
        return alert('請再確認一次新密碼！');
      }
    } else if (npw.value !== '') {
      if (oldpw.value === '') {
        return alert('請輸入舊密碼值！');
      } else if (cpw.value === '') {
        return alert('請再確認一次新密碼！');
      }
    } else if (cpw.value !== '') {
      if (oldpw.value === '') {
        return alert('請輸入舊密碼值！');
      } else if (npw.value === '') {
        return alert('請輸入新密碼值！');
      }
    }

    if (cpw.value !== form.newpw) {
      return alert('密碼資料不一致！');
    }

    /* 保存資料 */

    if (Object.keys(form).length > 0) {
      this.dataChange = this.commonservice.formChanges(form, this.memData);
      this.commonservice.editSingleData(form.userid, this.dataChange, this.saveUrl)
        .subscribe(data => {
          if (data.status === '0') {
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
