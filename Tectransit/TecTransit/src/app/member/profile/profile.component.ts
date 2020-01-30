import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';
import { AccountInfo } from 'src/app/_Helper/models';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  memForm: FormGroup;

  getdataUrl = '/api/Member/GetMemData';
  saveUrl = '/api/Member/EditMemData';

  memData: AccountInfo;
  dataChange;

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
      username: ['', Validators.required],
      taxid: ['', Validators.required],
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

  SaveData(form) {

    // stop here if form is invalid
    if (this.memForm.invalid) {
      return alert('必填欄位不能為空白！');
    }

    const cpw = (document.getElementById('confirmpw')) as HTMLInputElement;
    if (cpw.value !== form.newpw) {
      return alert('密碼資料不一致！');
    }

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
