import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TradeEntryComponent } from './trade-entry.component';

describe('BuyEntryComponent', () => {
  let component: TradeEntryComponent;
  let fixture: ComponentFixture<TradeEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TradeEntryComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TradeEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
