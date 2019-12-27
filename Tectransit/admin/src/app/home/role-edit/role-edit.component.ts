import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, FormsModule, Validators } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';
import { RoleInfo } from 'src/app/_Helper/models';


@Component({
  selector: 'app-role-edit',
  templateUrl: './role-edit.component.html',
  styleUrls: ['./role-edit.component.css']
})
export class RoleEditComponent implements OnInit {

  /* Web api url */
  private baseUrl = window.location.origin + '/api/SysHelp/';
  private roleUrl = 'GetTSRoleData';
  private roleEditUrl = 'EditTSRoleData';

  dataChange;
  dataForm: FormGroup;
  roleID = '';
  roleData: RoleInfo;
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
      roleid: 0,
      rolecode: ['', Validators.required],
      rolename: '',
      roledesc: '',
      roleseq: '',
      isenable: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: ''
    });

    // get parameters id then get data
    this.roleID = this.route.snapshot.paramMap.get('id');
    // 禁止修改系統最高權限
    if (this.roleID === '1') {
      this.router.navigate(['/roles']);
    }

    if (this.roleID !== '0') {
      this.getData(this.roleID);
    }
  }

  getData(id) {
    this.commonService.getSingleData(id, this.baseUrl + this.roleUrl)
      .subscribe(data => {
        this.dataForm.patchValue(data.rows);
        this.dataForm.controls.rolecode.disable();
        this.roleData = data.rows;
      },
        error => {
          console.log(error);
        });
  }

  saveData(form) {

    // check Form
    if (this.dataForm.invalid) {
      this.isErr = true;
      return alert('權限組代碼不能為空！');
    }

    if (this.roleID !== '0') {
      this.dataChange = this.commonService.formChanges(form, this.roleData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: this.roleID, formdata: this.dataChange };
      this.commonService.editSingleData(postData, this.baseUrl + this.roleEditUrl)
        .subscribe(data => {
          alert(data.msg);
        },
          error => {
            console.log(error);
          });
    } else { alert('頁面無數據被修改！'); }
  }

}
