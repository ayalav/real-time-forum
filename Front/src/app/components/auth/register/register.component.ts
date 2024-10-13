import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { Register } from '../../models/register';

@Component({
  selector: 'app-register',
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
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  registerForm: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {
    // Initialize form
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator  // Custom validator to check password match
    });

    // Check if user is already logged in, and redirect to the forum if true
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      if (isLoggedIn) {
        this.router.navigate(['/forum']);
      }
    });
  }

  // Custom validator to check if passwords match
  passwordMatchValidator(formGroup: FormGroup) {
    const password = formGroup.get('password')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const registerData: Register = {
        email: this.registerForm.get('email')?.value,
        username: this.registerForm.get('username')?.value,
        password: this.registerForm.get('password')?.value
      };
      this.isLoading = true;

      this.authService.register(registerData).subscribe(
        success => {
          this.isLoading = false;
          if (success) {
            this.snackBar.open('Registration successful! Please log in.', 'Close', { duration: 3000 });
            this.router.navigate(['/login']);
          } else {
            this.snackBar.open('Registration failed. Please try again.', 'Close', { duration: 3000 });
          }
        },
        error => {
          this.isLoading = false;
          this.snackBar.open('Registration failed due to server error. Please try again.', 'Close', { duration: 3000 });
        }
      );
    }
  }
}

