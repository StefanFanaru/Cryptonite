// Deep compare of two arrays (compares nested arrays and objects also)
import { compareObjects } from '../common/utils/utils';

declare global {
  interface Array<T> {
    equals(array: any[]): boolean;
  }
}

Array.prototype.equals = function (array: any[]): boolean {
  if (!array) return false;

  // compare lengths - can save a lot of time
  if (this.length !== array.length) return false;

  for (let i = 0, l = this.length; i < l; i++) {
    // Check if we have nested arrays
    if (this[i] instanceof Array && array[i] instanceof Array) {
      // recurse into the nested arrays
      if (!this[i].equals(array[i])) return false;
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
