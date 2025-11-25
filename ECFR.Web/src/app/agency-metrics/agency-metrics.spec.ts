import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyMetrics } from './agency-metrics';

describe('AgencyMetrics', () => {
  let component: AgencyMetrics;
  let fixture: ComponentFixture<AgencyMetrics>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AgencyMetrics]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AgencyMetrics);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
