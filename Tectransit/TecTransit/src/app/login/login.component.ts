import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../services/login.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
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
    private authenticationService: AuthenticationService
  ) {
    // redirect to home if already logged in
    const currentAcct = sessionStorage.getItem('currentAcct');
    if (currentAcct) {
      this.router.navigate(['/']);
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
    this.authenticationService.login(this.f.username.value, this.f.password.value, this.f.captcha.value)
      .pipe(first())
      .subscribe(
        data => {
          if (data.status === 'success') {
            document.location.href = this.returnUrl;
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
