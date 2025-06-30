import { TestBed } from '@angular/core/testing';

import { ProjectManagmentService } from './project-managment.service';

describe('ProjectManagmentService', () => {
  let service: ProjectManagmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectManagmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
