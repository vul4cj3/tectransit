import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountInfo } from 'src/app/_Helper/models';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-accounts-edit',
  templateUrl: './accounts-edit.component.html',
  styleUrls: ['./accounts-edit.component.css']
})
export class AccountsEditComponent implements OnInit {

  /* Web api url */
  private baseUrl = window.location.origin + '/api/UserHelp/';
  private userUrl = 'GetTSAccountData';
  private userEditUrl = 'EditTSAccountData';

  dataChange;
  dataForm: FormGroup;
  userID = '';
  userData: AccountInfo;
  isErr = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      userid: 0,
      usercode: ['', Validators.required],
      warehouseno: '',
      username: '',
      userdesc: '',
      userseq: '',
      email: '',
      taxid: '',
      phone: '',
      mobile: '',
      address: '',
      isenable: '1',
      lastlogindate: '',
      logincount: '',
      credate: '',
      creby: '',
      upddate: '',
      updby: ''
    });

    // get parameters id then get data
    this.userID = this.route.snapshot.paramMap.get('id');
    // 不開放新增功能
    if (this.userID === '0') {
      this.router.navigate(['/accounts']);
    }

    if (this.userID !== '0') {
      this.getData(this.userID);
    }
  }

  getData(id) {
    this.commonService.getSingleData(id, this.baseUrl + this.userUrl)
      .subscribe(data => {
        this.dataForm.patchValue(data.rows);
        this.dataForm.controls.usercode.disable();
        this.userData = data.rows;
      },
        error => {
          console.log(error);
        });
  }

  saveData(form) {

    // check Form
    if (this.dataForm.invalid) {
      this.isErr = true;
      return alert('用戶代碼不能為空！');
    }

    if (this.userID !== '0') {
      this.dataChange = this.commonService.formChanges(form, this.userData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: this.userID, formdata: this.dataChange };
      this.commonService.editSingleData(postData, this.baseUrl + this.userEditUrl)
        .subscribe(data => {
          alert(data.msg);
        },
          error => {
            console.log(error);
          });
    } else { alert('頁面無數據被修改！'); }
  }


}
