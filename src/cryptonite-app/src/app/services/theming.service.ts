import { ApplicationRef, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class ThemingService {
  themes = ['dark-theme', 'light-theme'];
  theme = new BehaviorSubject('dark-theme');
  darkModeOn: boolean;

  constructor(private ref: ApplicationRef) {
    const currentDarkMode = localStorage.getItem('dark_mode');
    if (currentDarkMode) {
      this.darkModeOn = JSON.parse(currentDarkMode);
    } else {
      this.darkModeOn = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    }

    if (!this.darkModeOn) {
      this.theme.next('light-theme');
    }

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', event => {
      const turnOn = event.matches;
      this.theme.next(turnOn ? 'dark-theme' : 'light-theme');

      // Trigger refresh of UI
      this.ref.tick();
    });
  }

  toggle() {
    this.theme.next(this.darkModeOn ? 'light-theme' : 'dark-theme');
    this.darkModeOn = !this.darkModeOn;
    localStorage.setItem('dark_mode', JSON.stringify(this.darkModeOn));
  }
}
