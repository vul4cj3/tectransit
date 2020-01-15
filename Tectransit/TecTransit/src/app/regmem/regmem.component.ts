import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonService } from '../services/common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-regmem',
  templateUrl: './regmem.component.html',
  styleUrls: ['./regmem.component.css']
})
export class RegmemComponent implements OnInit {
  regForm: FormGroup;
  year = [];
  month = [];
  days = [];
  tyear = (new Date()).getFullYear();

  saveUrl = '/api/Member/SaveRegData';

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
      idcode: ['', Validators.required],
      birth: [''],
      email: ['', Validators.required],
      address: ['', Validators.required],
      tel: ['', Validators.required],
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

    // 生日資料處理
    const birY = (document.getElementById('b_year')) as HTMLSelectElement;
    const birM = (document.getElementById('b_mon')) as HTMLSelectElement;
    const birD = (document.getElementById('b_day')) as HTMLSelectElement;

    if (birM.value === '2' && parseInt(birD.value, 2) > 29) {
      return alert('生日日期選擇有誤！');
    } else if ((birM.value === '4' || birM.value === '6' || birM.value === '9' || birM.value === '11') && birD.value === '31') {
      return alert('生日日期選擇有誤！');
    }

    form.birth = birY.value + '/' + birM.value + '/' + birD.value;

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
