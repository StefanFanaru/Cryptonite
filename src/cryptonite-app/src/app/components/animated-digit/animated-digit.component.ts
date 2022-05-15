import { AfterViewInit, Component, ElementRef, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';

@Component({
  selector: 'app-animated-digit',
  templateUrl: 'animated-digit.component.html',
  styleUrls: ['animated-digit.component.scss']
})
export class AnimatedDigitComponent implements AfterViewInit, OnChanges {
  @Input() duration: number;
  @Input() endValue: number;
  @Input() steps: number;
  @Input() suffix: string;
  @Input() positiveOnly: boolean;
  @ViewChild('animatedDigit') animatedDigit: ElementRef;

  oldValue: number;

  animateCount(): void {
    if (!this.duration) {
      this.duration = 1000;
    }

    if (typeof this.endValue === 'number') {
      this.counterFunc(this.endValue, this.duration, this.animatedDigit);
    }
  }

  counterFunc(endValue, durationMs, element): void {
    if (!element) {
      return;
    }

    if (!this.steps) {
      this.steps = 12;
    }

    if (Math.abs(endValue - this.oldValue) <= 0.01) {
      return;
    }

    const stepCount = Math.abs(durationMs / this.steps);
    const valueIncrement = (endValue - this.oldValue) / stepCount;
    const sinValueIncrement = Math.PI / stepCount;

    let currentValue = this.oldValue;
    let currentSinValue = 0;

    const step = (): void => {
      currentSinValue += sinValueIncrement;
      currentValue += valueIncrement * Math.sin(currentSinValue) ** 2 * 2;

      element.nativeElement.textContent = `${Math.abs(Math.round(currentValue * 100) / 100).toFixed(2)} ${this.suffix ?? ''}`;

      if (currentSinValue < Math.PI) {
        window.requestAnimationFrame(step);
      }
    };

    step();
  }

  ngAfterViewInit(): void {
    if (this.endValue) {
      this.animateCount();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.endValue) {
      this.oldValue = changes.endValue.previousValue ?? this.endValue;
      this.animateCount();
    }
  }
}
