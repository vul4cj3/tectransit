import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-shipping-header',
  templateUrl: './shipping-header.component.html',
  styleUrls: ['./shipping-header.component.css']
})
export class ShippingHeaderComponent implements OnInit {

  @Input()
  type: string;

  constructor() { }

  ngOnInit() {
  }

}
