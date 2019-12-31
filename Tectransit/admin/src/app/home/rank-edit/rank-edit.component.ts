import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RankInfo } from 'src/app/_Helper/models';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-rank-edit',
  templateUrl: './rank-edit.component.html',
  styleUrls: ['./rank-edit.component.css']
})
export class RankEditComponent implements OnInit {

  /* Web api url */
  private baseUrl = window.location.origin + '/api/UserHelp/';
  private rankUrl = 'GetTSRankData';
  private rankEditUrl = 'EditTSRankData';

  dataChange;
  dataForm: FormGroup;
  rankID = '';
  rankData: RankInfo;
  isErr = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      rankid: 0,
      rankcode: ['', Validators.required],
      rankname: '',
      ranktype: '1',
      rankdesc: '',
      rankseq: '',
      isenable: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: ''
    });

    // get parameters id then get data
    this.rankID = this.route.snapshot.paramMap.get('id');

    if (this.rankID !== '0') {
      this.getData(this.rankID);
    }
  }

  getData(id) {
    this.commonService.getSingleData(id, this.baseUrl + this.rankUrl)
      .subscribe(data => {
        this.dataForm.patchValue(data.rows);
        this.dataForm.controls.rankcode.disable();
        this.rankData = data.rows;
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

    if (this.rankID !== '0') {
      this.dataChange = this.commonService.formChanges(form, this.rankData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: this.rankID, formdata: this.dataChange };
      this.commonService.editSingleData(postData, this.baseUrl + this.rankEditUrl)
        .subscribe(data => {
          alert(data.msg);
        },
          error => {
            console.log(error);
          });
    } else { alert('頁面無數據被修改！'); }
  }

}
