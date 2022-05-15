import { DateTime } from 'luxon';

export function getDateFromLocale(input: any) {
  return DateTime.fromISO(new Date(input.toString()).toISOString()).toUTC().toString();
}
