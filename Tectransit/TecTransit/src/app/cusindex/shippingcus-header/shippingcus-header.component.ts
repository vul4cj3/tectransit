import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-shippingcus-header',
  templateUrl: './shippingcus-header.component.html',
  styleUrls: ['./shippingcus-header.component.css']
})
export class ShippingcusHeaderComponent implements OnInit {

  @Input()
  type: string;

  constructor() { }

  ngOnInit() {
  }

}
