import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListTradeEntriesComponent } from './list-trade-entries.component';

describe('ListBuysComponent', () => {
  let component: ListTradeEntriesComponent;
  let fixture: ComponentFixture<ListTradeEntriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListTradeEntriesComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListTradeEntriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
