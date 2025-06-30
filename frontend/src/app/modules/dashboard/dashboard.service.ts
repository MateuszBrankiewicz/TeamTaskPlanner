import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
interface Task {
  id: number;
  title: string;
  description: string;
  createdDate: string;
  dueDate: string;
  priority: string;
  status: string;
}
@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  http = inject(HttpClient);
  constructor() { }
  getDashboard(){
    return this.http.get<Task[]>("http://localhost:5078/api/dashboard/tasks",{withCredentials:true})
  }
  changeTaskAsDone(task : Task){
    return this.http.patch<Task>("http://localhost:5078/api/dashboard/tasks",task,{withCredentials:true});
  }
}
