import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CusloginService } from '../services/cuslogin.service';
import { CommonService } from '../services/common.service';

@Component({
  selector: 'app-cuslogin',
  templateUrl: './cuslogin.component.html',
  styleUrls: ['./cuslogin.component.css']
})
export class CusloginComponent implements OnInit {

  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error: string;
  isErr = false;

  public catpchaImg: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private cusloginservice: CusloginService,
    private commonservice: CommonService
  ) {
    // redirect to home if already logged in
    const currentCus = sessionStorage.getItem('currentCus');
    if (currentCus) {
      this.router.navigate(['/cus/profile']);
    }
  }

  ngOnInit() {
    this.resetForm();
    this.refreshCaptcha();
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/member/profile';
  }

  resetForm() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      captcha: ['', Validators.required]
    });
  }

  refreshCaptcha() {
    this.catpchaImg = `/api/Login/GetCaptcha?${Date.now()}`;
  }

  Clear() {
    this.resetForm();
    this.refreshCaptcha();
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.cusloginservice.login(this.f.username.value, this.f.password.value, this.f.captcha.value)
      .pipe(first())
      .subscribe(
        data => {
          if (data.status === 'success') {
            this.router.navigate(['/cus/profile']);
          } else {
            this.refreshCaptcha();
            this.isErr = true;
            alert(data.message);
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }

}
