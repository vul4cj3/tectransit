import { Component, Inject, HostListener } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  isScroll = false;

  constructor(@Inject(DOCUMENT) private document: Document, ) {

  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    if (document.body.scrollTop > 0 || document.documentElement.scrollTop > 0) {
      this.isScroll = true;
    } else if (document.body.scrollTop === 0 || document.documentElement.scrollTop === 0) {
      this.isScroll = false;
    }
  }

  clearnav() {
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < document.getElementsByClassName('active').length; i++) {
      const item = document.getElementsByClassName('active')[i];
      item.classList.remove('active');
    }

    const profileitem = document.getElementsByClassName('open')[0];
    if (profileitem !== undefined) {
      profileitem.classList.remove('open');
    }
  }

  gotoTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth'
    });
  }

}
