﻿/* eslint-disable prefer-arrow/prefer-arrow-functions */
export function hex(c) {
  const s = '0123456789abcdef';
  let i = parseInt(c, 10);
  if (i === 0 || isNaN(c)) {
    return '00';
  }
  i = Math.round(Math.min(Math.max(0, i), 255));
  return s.charAt((i - (i % 16)) / 16) + s.charAt(i % 16);
}

/* Convert an RGB triplet to a hex string */
export function convertToHex(rgb) {
  return hex(rgb[0]) + hex(rgb[1]) + hex(rgb[2]);
}

/* Remove '#' in color hex string */
export function trim(s) {
  return s.charAt(0) === '#' ? s.substring(1, 7) : s;
}

/* Convert a hex string to an RGB triplet */
export function convertToRGB(hexColor) {
  const color = [];
  color[0] = parseInt(trim(hexColor).substring(0, 2), 16);
  color[1] = parseInt(trim(hexColor).substring(2, 4), 16);
  color[2] = parseInt(trim(hexColor).substring(4, 6), 16);
  return color;
}

export function generateColor(colorStart, colorEnd, colorCount) {
  // The beginning of your gradient
  const start = convertToRGB(colorStart);

  // The end of your gradient
  const end = convertToRGB(colorEnd);

  // The number of colors to compute
  const len = colorCount;

  //Alpha blending amount
  let alpha = 0.0;

  const saida = [];

  for (let i = 0; i < len; i++) {
    const c = [];
    alpha += 1.0 / len;

    c[0] = start[0] * alpha + (1 - alpha) * end[0];
    c[1] = start[1] * alpha + (1 - alpha) * end[1];
    c[2] = start[2] * alpha + (1 - alpha) * end[2];

    saida.push(`#${convertToHex(c)}`);
  }

  return saida;
}
