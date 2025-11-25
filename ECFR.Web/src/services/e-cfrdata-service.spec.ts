import { TestBed } from '@angular/core/testing';

import { EcfrDataService } from './e-cfrdata-service';

describe('EcfrDataService', () => {
  let service: EcfrDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EcfrDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
