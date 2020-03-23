import { Component, OnInit, AfterViewChecked, DoCheck } from '@angular/core';
import { FaqCate, FaqInfo } from '../../_Helper/models';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.css']
})
export class FaqComponent implements OnInit {

  /* web api url */
  cateUrl = '/api/FrontHelp/GetFaqCate';
  dataUrl = '/api/FrontHelp/GetFaqData';

  faqCate: FaqCate[];
  faqData: FaqInfo[];

  isActive = true; // first enter first-category active(default)

  constructor(
    private commonservie: CommonService
  ) { }

  ngOnInit() {
    this.getCate();
  }

  getCate() {
    this.commonservie.getSingleData(this.cateUrl)
      .subscribe(data => {
        this.faqCate = data.rows;
        this.getData(this.faqCate[0].cateid);

      }, error => {
        console.log(error);
      });
  }

  getData(id) {
    this.commonservie.getData(id, this.dataUrl)
      .subscribe(data => {
        if (data.rows !== '') {
          this.faqData = data.rows;
        } else {
          this.faqData = null;
        }

      }, error => {
        console.log(error);
      });
  }

  chgFaq(id, e) {
    this.isActive = false;
    const controls = document.getElementsByClassName('cate');
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < controls.length; i++) {
      const subitem = controls[i].lastChild as HTMLSpanElement;
      subitem.classList.remove('active');
    }

    const control = e.target as HTMLSpanElement;
    control.classList.add('active');

    this.getData(id);
  }

  toggleFaq(id) {
    const control = document.getElementById(id);
    const test = control.classList.contains('active');
    if (test) {
      control.classList.remove('active');
    } else {
      control.classList.add('active');
    }

  }

}
