import { TestBed } from '@angular/core/testing';

import { ApiHttpClientService } from './api-http-client.service';

describe('ApiHttpClientService', () => {
  let service: ApiHttpClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApiHttpClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
