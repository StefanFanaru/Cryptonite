/* eslint-disable prefer-arrow/prefer-arrow-functions */
import { AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

export function getGUID() {
  // @ts-ignore
  return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
    (c ^ (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))).toString(16)
  );
}

export const MY_FORMATS = {
  parse: {
    dateInput: 'LL'
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY'
  }
};

export function isMobile() {
  return (
    /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(
      navigator.userAgent
    ) ||
    /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(
      navigator.userAgent.substr(0, 4)
    ) ||
    window.innerWidth <= 900
  );
}

export function isDesktop() {
  return window.innerWidth > 1024;
}

export function copy(aObject) {
  if (!aObject) {
    return aObject;
  }

  let v;
  const bObject = Array.isArray(aObject) ? [] : {};
  for (const k in aObject) {
    v = aObject[k];
    bObject[k] = typeof v === 'object' ? copy(v) : v;
  }

  return bObject;
}

export function dayOfWeekAsString(dayIndex) {
  return ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'][dayIndex] || '';
}

// I found multiple obstacles in Angular with assigning this to Object.prototype
export function compareObjects(object1, object2) {
  for (const key in object1) {
    // Check existence of prop on both objects
    if (object1.hasOwnProperty(key) != object2.hasOwnProperty(key)) {
      return false;
    }
    const value1 = object1[key];
    const value2 = object2[key];

    // Recurse into the nested arrays
    if (value1 instanceof Array && value2 instanceof Array) {
      if (!value1.equals(value2)) {
        return false;
      }
    }

    // Recurse into the nested objects
    if (value1 instanceof Object && value2 instanceof Object) {
      return this.compareObjects(value1, value2);
    }

    // Skip evaluation of differences between null, undefined or empty string (no negative effect if booleans)
    if (!value1 && !value2) {
      continue;
    }
    if (value1 !== value2) {
      return false;
    }
  }
  return true;
}

export function hoursAndMinutes(hours, minutes) {
  return `${hours > 9 ? hours : `0${hours}`}:${minutes > 9 ? minutes : `0${minutes}`}`;
}

export function makeSimpleDateString(date: Date) {
  return `${date.getDate() > 9 ? date.getDate() : `0${date.getDate()}`}/${
    date.getMonth() + 1 > 9 ? date.getMonth() + 1 : `0${date.getMonth() + 1}`
  }/${date.getFullYear()} ${hoursAndMinutes(date.getHours(), date.getMinutes())}`;
}

export function areArraysEqual(defaultValue: any[], newValue: any[]): boolean {
  if (defaultValue && defaultValue.length) {
    return defaultValue.equals(newValue);
  }
  return (!defaultValue && !newValue) || defaultValue?.length === newValue?.length;
}

export function filterStrings(value: string, options: string[]): string[] {
  const filterValue = value.toLowerCase();
  return options.filter(option => option.toLowerCase().includes(filterValue));
}

export function observeAutocompleteChanges(control: AbstractControl, options: string[]): Observable<string[]> {
  return control.valueChanges.pipe(
    startWith(''),
    map(value => filterStrings(value, options))
  );
}

declare global {
  interface Array<T> {
    equals(array: any[]): boolean;
  }
}

Array.prototype.equals = function (array: any[]): boolean {
  if (!array) {
    return false;
  }

  // compare lengths - can save a lot of time
  if (this.length !== array.length) {
    return false;
  }

  for (let i = 0, l = this.length; i < l; i++) {
    // Check if we have nested arrays
    if (this[i] instanceof Array && array[i] instanceof Array) {
      // recurse into the nested arrays
      if (!this[i].equals(array[i])) {
        return false;
      }
    }

    // Check if object comparison is required
    else if (this[i] instanceof Object && array[i] instanceof Object) {
      if (!compareObjects(this[i], array[i])) {
        return false;
      }
    } else if (this[i] !== array[i]) {
      return false;
    }
  }
  return true;
};

// Hide method from for-in loops
Object.defineProperty(Array.prototype, 'equals', { enumerable: false });

export {};
