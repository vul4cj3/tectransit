import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../services/login.service';
import { CommonService } from '../services/common.service';

@Component({
  selector: 'app-forgetpw',
  templateUrl: './forgetpw.component.html',
  styleUrls: ['./forgetpw.component.css']
})
export class ForgetpwComponent implements OnInit {

  dataForm: FormGroup;
  error: string;
  isErr = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private commonservice: CommonService
  ) {
    // redirect to home if already logged in
    const currentAcct = sessionStorage.getItem('currentAcct');
    if (currentAcct) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.resetForm();
  }

  resetForm() {
    this.dataForm = this.formBuilder.group({
      idcode: ['', Validators.required],
      email: ['', Validators.required]
    });
  }

  Clear() {
    this.resetForm();
  }

  onSubmit(form) {
    // stop here if form is invalid
    if (this.dataForm.invalid) {
      return alert('欄位不得為空白');
    }

    this.commonservice.GetNewPw(form)
      .subscribe((data) => {
        this.resetForm();
        alert(data.msg);
      }, error => {
        console.log(error);
      });

  }

}
