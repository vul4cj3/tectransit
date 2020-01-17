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
      return '已併貨';
    } else if (value === '3') {
      return '已完成';
    } else { }

    return null;
  }

}
