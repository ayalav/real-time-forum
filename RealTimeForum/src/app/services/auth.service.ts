import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of } from 'rxjs';
import { Register } from '../models/register';
import { Login } from '../models/login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7260/auth';
  private isLoggedInSubject!: BehaviorSubject<boolean>;

  constructor(
    private http: HttpClient
  ) {
    const token = localStorage.getItem('authToken');
    this.isLoggedInSubject = new BehaviorSubject<boolean>(!!token);
  }

  // Sends login request and updates login status if successful
  login(loginData: Login): Observable<boolean> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, loginData).pipe(
      map(response => {
        if (response && response.token) {
          localStorage.setItem('authToken', response.token);  // Store the token
          this.isLoggedInSubject.next(true);
          return true;
        }
        return false;
      }),
      catchError(error => {
        console.error('Login failed', error);
        this.isLoggedInSubject.next(false);
        return of(false);
      })
    );
  }

  // Sends registration request 
  register(registerData: Register): Observable<boolean> {
    return this.http.post(`${this.apiUrl}/register`, registerData).pipe(
      map(response => {
        return true;
      }),
      catchError(error => {
        console.error('Registration failed', error);
        return of(false);
      })
    );
  }

  // Observable to expose the current login status
  get isLoggedIn$(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  // Logs out the user by removing the token
  logout(): void {
    localStorage.removeItem('authToken');
  }
}