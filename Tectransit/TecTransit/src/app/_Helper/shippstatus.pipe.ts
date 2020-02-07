import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'shippstatus'
})
export class ShippstatusPipe implements PipeTransform {

  transform(value: string): any {
    if (value === '0') {
      return '未入庫';
    } else if (value === '1') {
      return '已入庫';
    } else if (value === '2') {
      return '待出貨';
    } else if (value === '3') {
      return '已出貨';
    } else if (value === '4') {
      return '已完成';
    } else { }

    return null;
  }

}
