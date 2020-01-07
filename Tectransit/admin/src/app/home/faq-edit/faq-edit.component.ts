import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FaqInfo } from 'src/app/_Helper/models';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-faq-edit',
  templateUrl: './faq-edit.component.html',
  styleUrls: ['./faq-edit.component.css']
})
export class FaqEditComponent implements OnInit {

  /* Web api url */
  private baseUrl = window.location.origin + '/api/WebSetHelp/';
  private userUrl = 'GetFaqData';
  private userEditUrl = 'EditTDFaqData';

  public config = this.commonService.editorConfig;

  dataChange;
  dataForm: FormGroup;
  cateID: string;
  faqID = '';
  FaqData: FaqInfo;
  isErr = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    // get parameters id then get data
    this.cateID = this.route.snapshot.paramMap.get('id');
    this.faqID = this.route.snapshot.paramMap.get('id2');

    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      faqid: 0,
      title: ['', Validators.required],
      descr: '',
      faqseq: '0',
      istop: '0',
      isenable: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: '',
      cateid: this.cateID
    });

    if (this.faqID !== '0') {
      this.getData(this.faqID);
    }
  }

  getData(id) {
    this.commonService.getSingleData(id, this.baseUrl + this.userUrl)
      .subscribe(data => {
        this.dataForm.patchValue(data.rows);
        this.FaqData = data.rows;
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

    if (this.faqID !== '0') {
      this.dataChange = this.commonService.formChanges(form, this.FaqData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: this.faqID, formdata: this.dataChange };
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
