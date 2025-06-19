import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {tap} from 'rxjs';

interface User {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  private httpClient = inject(HttpClient);
  constructor() { }
  registerUser(user : User){
    return this.httpClient.post<User>('http://localhost:5078/api/auth/register',user)
  }
  loginUser(user: User){
    return this.httpClient.post<User>('http://localhost:5078/api/auth/login',user,{withCredentials:true}).pipe(
      tap(res=>{
        console.log(res);
      })
    )
  }
}
