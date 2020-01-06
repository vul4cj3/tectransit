import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NewsInfo } from 'src/app/_Helper/models';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-news-edit',
  templateUrl: './news-edit.component.html',
  styleUrls: ['./news-edit.component.css']
})
export class NewsEditComponent implements OnInit {

  /* Web api url */
  private baseUrl = window.location.origin + '/api/WebSetHelp/';
  private userUrl = 'GetTDNewsData';
  private userEditUrl = 'EditTDNewsData';

  public editorConfig = {
    toolbarGroups: [{ name: 'document', groups: ['mode'] },
    { name: 'clipboard', groups: ['undo'] },
    { name: 'editing', groups: ['find', 'selection'] },
    { name: 'styles' },
      '/',
    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
    { name: 'colors', groups: ['TextColor', 'BGColor'] },
    { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },
    { name: 'links' },
    { name: 'insert' },
      '/'
    ],
    removeButtons: 'Strike,Subscript,Superscript,Anchor,Styles,Specialchar',
    extraPlugins: 'font',
    filebrowserBrowseUrl: '/ckfinder/ckfinder.html',
    filebrowserImageBrowseUrl: '/ckfinder/ckfinder.html?type=Images',
    filebrowserFlashBrowseUrl: '/ckfinder/ckfinder.html?type=Flash',
    filebrowserUploadUrl: '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
    filebrowserImageUploadUrl: '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',
    filebrowserFlashUploadUrl: '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash',
    height: 300
  };

  dataChange;
  dataForm: FormGroup;
  newsID = '';
  newsData: NewsInfo;
  isErr = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    // built form controls and default form value
    this.dataForm = this.formBuilder.group({
      newsid: 0,
      title: ['', Validators.required],
      descr: '',
      newsseq: '',
      upsdate: '',
      upedate: '',
      istop: '0',
      isenable: '1',
      credate: '',
      creby: '',
      upddate: '',
      updby: ''
    });

    // get parameters id then get data
    this.newsID = this.route.snapshot.paramMap.get('id');

    if (this.newsID !== '0') {
      this.getData(this.newsID);
    }
  }

  getData(id) {
    this.commonService.getSingleData(id, this.baseUrl + this.userUrl)
      .subscribe(data => {
        this.dataForm.patchValue(data.rows);
        this.dataForm.controls.usercode.disable();
        this.newsData = data.rows;
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

    if (this.newsID !== '0') {
      this.dataChange = this.commonService.formChanges(form, this.newsData);
    } else { this.dataChange = form; }

    if (Object.keys(this.dataChange).length > 0) {
      const postData = { id: this.newsID, formdata: this.dataChange };
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
