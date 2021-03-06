import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-regconfirm',
  templateUrl: './regconfirm.component.html',
  styleUrls: ['../register/register.component.css', './regconfirm.component.css']
})
export class RegconfirmComponent implements OnInit {

  regForm: FormGroup;
  userID: number;

  isActive = false;

  confirmUrl = '/api/Member/ConfirmEmailCode';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private commonservice: CommonService
  ) { }

  ngOnInit() {
    // get parameters id then get data
    if (this.route.snapshot.paramMap.get('id') === '') {
      this.router.navigate(['/']);
    }

    this.userID = Number(this.route.snapshot.paramMap.get('id'));
    this.resetForm();
  }

  resetForm() {
    this.regForm = this.formBuilder.group({
      userid: [this.userID],
      emailcode: ['', Validators.required]
    });
  }

  enableSend(e) {
    if (e.target.value.length > 0) {
      this.isActive = true;
    } else { this.isActive = false; }
  }

  ConfirmData(form) {
    // stop here if form is invalid
    if (this.regForm.invalid) {
      return '';
    }

    if (Object.keys(form).length > 0) {
      this.commonservice.insertData(form, this.confirmUrl)
        .subscribe(data => {
          if (data.status === '0') {
            // 往下一步:註冊完成頁
            this.router.navigate(['/regfinal']);
          } else {
            alert(data.msg);
          }

        }, error => {
          console.log(error);
        });

    }
  }

}
