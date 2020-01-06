import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AboutInfo } from 'src/app/_Helper/models';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-about-edit',
  templateUrl: './about-edit.component.html',
  styleUrls: ['./about-edit.component.css']
})
export class AboutEditComponent implements OnInit {
  /* Web api url */
  private baseUrl = window.location.origin + '/api/WebSetHelp/';
  private userUrl = 'GetAboutData';
  private userEditUrl = 'EditTDAboutData';

  public config = this.commonService.editorConfig;

  dataChange;
  dataForm: FormGroup;
  cateID: string;
  aboutID = '';
  AboutData: AboutInfo;
  isErr = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    // get parameters id then get data
    this.cateID = this.route.snapshot.paramMap.get('id');
    this.aboutID = this.route.snapshot.paramMap.get('id2');

    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      aboutid: 0,
      title: ['', Validators.required],
      descr: '',
      aboutseq: '0',
      istop: '0',
      isenable: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: '',
      cateid: this.cateID
    });

    if (this.aboutID !== '0') {
      this.getData(this.aboutID);
    }
  }

  getData(id) {
    this.commonService.getSingleData(id, this.baseUrl + this.userUrl)
      .subscribe(data => {
        this.dataForm.patchValue(data.rows);
        this.AboutData = data.rows;
      },
        error => {
          console.log(error);
        });
  }

  saveData(form) {
    // check Form
    if (this.dataForm.invalid) {
      this.isErr = true;
      return alert('標題不能為空！');
    }

    if (this.aboutID !== '0') {
      this.dataChange = this.commonService.formChanges(form, this.AboutData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: this.aboutID, formdata: this.dataChange };
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
