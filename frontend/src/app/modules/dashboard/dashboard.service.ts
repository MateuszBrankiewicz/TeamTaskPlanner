import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  http = inject(HttpClient);
  constructor() { }
  getDashboard(){
    return this.http.get("http://localhost:5078/api/dashboard/tasks",{withCredentials:true})
  }
}
