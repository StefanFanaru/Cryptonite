import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogInjectableComponent } from './dialog-injectable.component';

describe('DialogInjectableComponent', () => {
  let component: DialogInjectableComponent;
  let fixture: ComponentFixture<DialogInjectableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DialogInjectableComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogInjectableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
