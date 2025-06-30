import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CreateProject } from './project-types';

@Injectable({
  providedIn: 'root'
})
export class ProjectManagmentService {
  htpp = inject(HttpClient);

  createProject(project: CreateProject){
    return this.htpp.post("http://localhost:5078/api/project",project,{withCredentials:true})
  }
}
