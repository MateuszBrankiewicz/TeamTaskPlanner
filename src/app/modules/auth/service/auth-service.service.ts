import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

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
    this.httpClient.post<User>('http://localhost:3000/users/register',user).subscribe(user => {
      console.log(user);
    })
  }
}
