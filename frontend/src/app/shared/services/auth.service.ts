import { Injectable, signal } from '@angular/core';

interface UserDetails{
  userName:string;
  isLoggedIn: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  authUser = signal<UserDetails>({
    userName: "",
    isLoggedIn: false
  });
  constructor() { }
}
