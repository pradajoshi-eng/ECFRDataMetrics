import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyList } from './agency-list';

describe('AgencyList', () => {
  let component: AgencyList;
  let fixture: ComponentFixture<AgencyList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AgencyList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AgencyList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
