import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'shippstatus'
})
export class ShippstatusPipe implements PipeTransform {

  transform(value: number, type: string = 's'): any {
    if (type === 'pay') {
      if (value === 0) {
        return '未付款';
      } else if (value === 1) {
        return '付款成功';
      } else if (value === 2) {
        return '付款失敗';
      } else { }
    } else if (type === 'check') {
      if (value === 0) {
        return '';
      } else if (value === 1) {
        return '已點收到';
      } else if (value === 2) {
        return '未點收到';
      } else if (value === 3) {
        return '其他';
      } else { }
    } else {
      if (value === 0) {
        return '未入庫';
      } else if (value === 1) {
        return '已入庫';
      } else if (value === 2) {
        return '待出貨';
      } else if (value === 3) {
        return '已出貨';
      } else if (value === 4) {
        return '已完成';
      } else { }
    }

    return null;
  }

}
