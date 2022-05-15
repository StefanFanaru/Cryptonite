import { Pipe, PipeTransform } from '@angular/core';
import { dayOfWeekAsString, hoursAndMinutes, makeSimpleDateString } from '../utils/utils';

@Pipe({
  name: 'customDate'
})
export class CustomDatePipe implements PipeTransform {
  transform(value: Date): string {
    let date: Date = new Date(Date.parse(value.toString()));
    let now: Date = new Date();
    let hours = date.getHours();
    let minutes = date.getMinutes();

    if (now.getDate() - date.getDate() > 4 || now.getMonth() != date.getMonth()) {
      return makeSimpleDateString(date);
    }

    if (date.getDate() == now.getDate() && date.getMonth() == now.getMonth()) {
      return hoursAndMinutes(hours, minutes);
    }

    return `${dayOfWeekAsString(date.getDay())}, ${hoursAndMinutes(hours, minutes)}`;
  }
}
