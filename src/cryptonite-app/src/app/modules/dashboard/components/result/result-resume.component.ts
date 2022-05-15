import { AfterViewInit, Component, Input, OnChanges, OnInit, QueryList, SimpleChanges, ViewChild, ViewChildren } from '@angular/core';
import { ResultData } from '../../../../models/portofolio/result-data';

@Component({
  selector: 'app-result-resume',
  templateUrl: './result-resume.component.html',
  styleUrls: ['./result-resume.component.scss']
})
export class ResultResumeComponent implements OnInit, OnChanges, AfterViewInit {
  @Input() data: ResultData;
  @ViewChild('oneItem') oneItem: any;
  @ViewChildren('count') count: QueryList<any>;
  now = new Date();
  timeoutId: NodeJS.Timer;

  constructor() {
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes || !changes.data.currentValue) {
      return;
    }
  }

  ngAfterViewInit(): void {
    // this.animateCount();
  }

  animateCount() {
    // eslint-disable-next-line @typescript-eslint/no-this-alias
    const context = this;

    const single = this.oneItem.nativeElement.innerHTML;

    this.counterFunc(single, this.oneItem, 7000);

    this.count.forEach(item => {
      context.counterFunc(item.nativeElement.innerHTML, item, 2000);
    });
  }

  counterFunc(end: number, element: any, duration: number) {
    const range = end - 0;
    const step = Math.abs(Math.floor(duration / range));
    let current = 0;

    const timer = setInterval(() => {
      current += 1;
      element.nativeElement.textContent = current;
      if (current === end) {
        clearInterval(timer);
      }
    }, step);
  }
}
