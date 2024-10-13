import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(
    private snackBar: MatSnackBar
  ) {}

  // General method to show snack bar
  show(message: string, action: string = 'Close', duration: number = 3000, panelClass: string = ''): void {
    this.snackBar.open(message, action, {
      duration: duration, 
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
      panelClass: panelClass ? [panelClass] : undefined
    });
  }

  // Success message
  showSuccess(message: string): void {
    this.show(message, 'Close', 3000, 'success-snackbar');
  }

  // Error message
  showError(message: string): void {
    this.show(message, 'Close', 5000, 'error-snackbar'); 
  }
}