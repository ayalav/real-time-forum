import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;

  constructor() { }

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

  //Listens for updates on comment
  public addCommentUpdateListener(): void {
    this.hubConnection.on('ReceiveCommentUpdate', (message) => {
      console.log('New comment update:', message);
      // כאן ניתן להציג הודעה למשתמש או לעדכן את התצוגה
    });
  }

  //Listens for updates on posts
  public addPostUpdateListener(): void {
    this.hubConnection.on('ReceivePostUpdate', (message) => {
      console.log('New post update:', message);
      // כאן ניתן להציג הודעה למשתמש או לעדכן את התצוגה
    });
  }
}
