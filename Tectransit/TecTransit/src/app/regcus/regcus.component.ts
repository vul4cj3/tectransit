import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from '../services/common.service';

@Component({
  selector: 'app-regcus',
  templateUrl: './regcus.component.html',
  styleUrls: ['./regcus.component.css']
})
export class RegcusComponent implements OnInit {

  regForm: FormGroup;
  year = [];
  month = [];
  days = [];
  tyear = (new Date()).getFullYear();

  saveUrl = '/api/Member/SaveRegCompanyData';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.resetForm();
    this.GetBirth();
  }

  resetForm() {
    this.regForm = this.formBuilder.group({
      usercode: ['', Validators.required],
      userpassword: ['', Validators.required],
      username: ['', Validators.required],
      companyname: ['', Validators.required],
      rateid: ['', Validators.required],
      email: ['', Validators.required],
      address: ['', Validators.required],
      phone: ['', Validators.required],
      mobile: ['', Validators.required]
    });
  }

  GetBirth() {
    let k = 0;
    for (let i = 1; i < 91; i++) {
      this.year[k] = this.tyear - i;
      k++;
    }

    for (let i = 0; i < 12; i++) {
      this.month[i] = i + 1;
    }

    for (let i = 0; i < 31; i++) {
      this.days[i] = i + 1;
    }

  }

  SaveData(form) {

    const chkArg = (document.getElementById('chkAgree')) as HTMLInputElement;
    if (chkArg.checked !== true) {
      return alert('請勾選同意會員條款！');
    }

    // stop here if form is invalid
    if (this.regForm.invalid) {
      return alert('必填欄位不能為空白！');
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
