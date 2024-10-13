import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;

  private postsSource = new BehaviorSubject<string>('');
  public posts$ = this.postsSource.asObservable();

  private commentsSource = new BehaviorSubject<string>('');
  public comments$ = this.commentsSource.asObservable();

  constructor(
    private notificationService: NotificationService
  ) { }

  //Starting a connection to the Hub
  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7260/forumHub')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  //Listens for updates on comments
  public addCommentUpdateListener(): void {
    this.hubConnection.on('ReceiveCommentUpdate', (message) => {
      this.commentsSource.next(message);
      this.notificationService.showSuccess(`New comment: ${message}`);
    });
  }

  //Listens for updates on posts
  public addPostUpdateListener(): void {
    this.hubConnection.on('ReceivePostUpdate', (message) => {
      this.postsSource.next(message);
      this.notificationService.showSuccess(`New post: ${message}`);
    });
  }
}
