import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CreateProject, Projects } from './project-types';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectManagmentService {
  http = inject(HttpClient);

  createProject(project: CreateProject){
    return this.http.post("http://localhost:5078/api/project",project,{withCredentials:true});
  }
  takeProjects(){
    return this.http.get<Projects[]>("http://localhost:5078/api/project", {withCredentials:true}).pipe(tap(res => console.log(res)));
  }
}
