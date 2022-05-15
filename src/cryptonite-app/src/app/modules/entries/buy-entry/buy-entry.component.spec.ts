import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyEntryComponent } from './buy-entry.component';

describe('BuyEntryComponent', () => {
  let component: BuyEntryComponent;
  let fixture: ComponentFixture<BuyEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BuyEntryComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BuyEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
