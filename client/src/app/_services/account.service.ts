import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

// always import fro, environment and not environment/development
// because if we do that, we will always be in development mode
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root', // inject into the app.module
})
export class AccountService {
  baseUrl = environment.apiUrl;
  // declare an observer amd observable
  private currentUserSource = new BehaviorSubject<User | null>(null);
  // exposing only observable part of the BehaviorSubject for security reasons
  currentUser$ = this.currentUserSource.asObservable();
  // getting http through DI
  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }
  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }
  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
