import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonService } from '../../services/common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-regmem',
  templateUrl: './regmem.component.html',
  styleUrls: ['../register/register.component.css', './regmem.component.css']
})
export class RegmemComponent implements OnInit {
  regForm: FormGroup;

  saveUrl = '/api/Member/SaveRegData';

  isShow = false;
  isShow2 = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm() {
    this.regForm = this.formBuilder.group({
      usercode: ['', Validators.required],
      userpassword: ['', Validators.required],
      username: ['', Validators.required],
      taxid: ['', Validators.required],
      email: ['', Validators.required],
      address: ['', Validators.required],
      phone: ['', Validators.required],
      mobile: ['', Validators.required]
    });
  }

  showPW(id) {
    const controls = document.getElementById(id) as HTMLInputElement;
    if (controls.type === 'password') {
      if (id === 'userpassword') {
        this.isShow = true;
      } else {
        this.isShow2 = true;
      }
      controls.type = 'text';
    } else {
      if (id === 'userpassword') {
        this.isShow = false;
      } else {
        this.isShow2 = false;
      }
      controls.type = 'password';
    }
  }

  SaveData(form) {

    const chkArg = (document.getElementById('chkAgree')) as HTMLInputElement;
    if (chkArg.checked !== true) {
      return alert('請勾選同意會員條款！');
    }

    // stop here if form is invalid
    if (this.regForm.invalid) {
      return alert('欄位不能為空白！');
    }

    const cpw = (document.getElementById('confirmpw')) as HTMLInputElement;
    if (cpw.value !== form.userpassword) {
      return alert('用戶密碼資料不一致！');
    }

    if (Object.keys(form).length > 0) {
      this.commonservice.insertData(form, this.saveUrl)
        .subscribe(data => {
          if (data.status === '0') {
            // 往下一步:驗證信箱
            this.router.navigate(['/regconfirm/' + data.id]);
          } else {
            alert(data.msg);
          }

        }, error => {
          console.log(error);
        });

    }
  }

}
