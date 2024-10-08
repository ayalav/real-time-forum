import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = '';  

  constructor(private http: HttpClient) {}

  // Sends login request and returns true if successful, false otherwise
  login(username: string, password: string): Observable<boolean> {
    const loginData = { username, password };

    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, loginData).pipe(
      map(response => {
        if (response && response.token) {
          localStorage.setItem('authToken', response.token);  // Store the token
          return true;
        }
        return false;
      }),
      catchError(error => {
        console.error('Login failed', error);
        return of(false);
      })
    );
  }

  // Checks if the user is logged in by verifying token presence
  isLoggedIn(): boolean {
    return !!localStorage.getItem('authToken');
  }

  // Logs out the user by removing the token
  logout(): void {
    localStorage.removeItem('authToken');
  }
}