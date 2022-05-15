import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListBuyEntriesComponent } from './list-buy-entries.component';

describe('ListBuysComponent', () => {
  let component: ListBuyEntriesComponent;
  let fixture: ComponentFixture<ListBuyEntriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListBuyEntriesComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListBuyEntriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
