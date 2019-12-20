import { Component, OnInit, OnChanges, Input, Output, EventEmitter } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit, OnChanges {

  @Output()
  pageChanged = new EventEmitter<any>();

  newPage = 0;
  pageSize = 10;

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
