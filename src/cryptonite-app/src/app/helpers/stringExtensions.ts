declare global {
  interface String {
    firstCharUpper(): string;

    formatKey(): string;

    toPascalCase(): string;

    toSnakeCase(): string;
  }
}

String.prototype.firstCharUpper = function () {
  let word = String(this);

  return word.charAt(0).toLocaleUpperCase() + word.substring(1, word.length);
};

String.prototype.formatKey = function () {
  let str = String(this);
  str = str.replace(/([a-z\xE0-\xFF])([A-Z\xC0\xDF])/g, '$1 $2');

  return str.toLocaleLowerCase().firstCharUpper();
};

String.prototype.toPascalCase = function () {
  return this.replace(/\w+/g, w => w[0].toUpperCase() + w.slice(1).toLowerCase());
};

String.prototype.toSnakeCase = function () {
  return this.match(/[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+/g)
    .map(x => x.toLowerCase())
    .join('_');
};

export {};
