import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingSummaryComponent } from './pending-summary.component';

describe('PendingSummaryComponent', () => {
  let component: PendingSummaryComponent;
  let fixture: ComponentFixture<PendingSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PendingSummaryComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PendingSummaryComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
