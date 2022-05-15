import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DiagnosticsAuthComponent } from './diagnostics-auth.component';

describe('DiagnosticsComponent', () => {
  let component: DiagnosticsAuthComponent;
  let fixture: ComponentFixture<DiagnosticsAuthComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DiagnosticsAuthComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DiagnosticsAuthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
