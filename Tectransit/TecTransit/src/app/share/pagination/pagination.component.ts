import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit, OnChanges {

  @Input()
  currentPage: number;
  @Input()
  preGroupApper: false;
  @Input()
  preApper: false;
  @Input()
  nextApper: false;
  @Input()
  nextGroupApper: false;
  @Input()
  pageNum: Array<number> = [];

  @Output()
  pageChanged = new EventEmitter<any>();

  newPage = 0;

  constructor(
    public pageService: CommonService
  ) { }

  ngOnInit() {
  }

  ngOnChanges() {
  }

  pageEdit(id: number) {
    this.newPage = id;
    this.pageChanged.emit(this.newPage);
  }
}
