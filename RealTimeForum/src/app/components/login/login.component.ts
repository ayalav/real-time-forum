import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatError } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { Login } from '../../models/login';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    HttpClientModule,  
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService) {
    // Initialize form
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],  
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

     // Redirect if already logged in
     if (this.authService.isLoggedIn()) {
      this.router.navigate(['/forum']);
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const loginData: Login = {
        username: this.loginForm.get('username')?.value,
        password: this.loginForm.get('password')?.value
      };
      this.isLoading = true;

      this.authService.login(loginData).subscribe(
        success => {
          this.isLoading = false;
          if (success) {
            this.router.navigate(['/forum']);
          } else {
            console.error('Invalid login credentials');
          }
        },
        error => {
          this.isLoading = false;
          console.error('Login failed', error);
        }
      );
    }
  }
}
