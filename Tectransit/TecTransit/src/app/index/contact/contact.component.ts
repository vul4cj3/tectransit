import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  /* web api url */
  dataUrl = '/api/CommonHelp/EditContact';

  dataForm: FormGroup;
  public catpchaImg: string;

  constructor(
    private formBuilder: FormBuilder,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    this.resetForm();
    this.refreshCaptcha();
  }

  resetForm() {
    this.dataForm = this.formBuilder.group({
      name: ['', Validators.required],
      phone: [''],
      email: ['', Validators.required],
      message: ['', Validators.required],
      captcha: ['', Validators.required]
    });
  }

  refreshCaptcha() {
    this.catpchaImg = `/api/CommonHelp/GetCaptcha?${Date.now()}`;
  }

  onSubmit(form) {

    // stop here if form is invalid
    if (this.dataForm.invalid) {
      return alert('請輸入必填欄位！');
    }

    this.commonservice.insertData(form, this.dataUrl)
      .subscribe(data => {
        if (data.status === '0') {
          this.resetForm();
          alert(data.msg);
        } else {
          alert(data.msg);
        }

      }, error => {
        console.log(error);
      });
  }

  Clear() {
    this.resetForm();
    this.refreshCaptcha();
  }

}
