import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FaqCate } from 'src/app/_Helper/models';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-faq-categoryedit',
  templateUrl: './faq-categoryedit.component.html',
  styleUrls: ['./faq-categoryedit.component.css']
})
export class FaqCategoryeditComponent implements OnInit {
  /* Web api url */
  private baseUrl = window.location.origin + '/api/WebSetHelp/';
  private userUrl = 'GetFaqCateData';
  private userEditUrl = 'EditTDFaqCateData';

  dataChange;
  dataForm: FormGroup;
  cateID = '';
  CateData: FaqCate;
  isErr = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      cateid: 0,
      title: ['', Validators.required],
      descr: '',
      faqseq: '',
      istop: '0',
      isenable: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: ''
    });

    // get parameters id then get data
    this.cateID = this.route.snapshot.paramMap.get('id');

    if (this.cateID !== '0') {
      this.getData(this.cateID);
    }
  }

  getData(id) {
    this.commonService.getSingleData(id, this.baseUrl + this.userUrl)
      .subscribe(data => {
        this.dataForm.patchValue(data.rows);
        this.CateData = data.rows;
      },
        error => {
          console.log(error);
        });
  }

  saveData(form) {
    // check Form
    if (this.dataForm.invalid) {
      this.isErr = true;
      return alert('分類標題不能為空！');
    }

    if (this.cateID !== '0') {
      this.dataChange = this.commonService.formChanges(form, this.CateData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: this.cateID, formdata: this.dataChange };
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
