import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {
  getPaginatedResult,
  getPaginationHeaders,
} from '../_helpers/paginationHelper';
import { Message } from '../_models/message';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../_models/user';
import { BehaviorSubject, take } from 'rxjs';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient) {}

  getMessages(pageSize: number, pageNumber: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);

    return getPaginatedResult<Message[]>(
      this.baseUrl + 'messages',
      params,
      this.http
    );
  }
  getMessageThread(username: string) {
    return this.http.get<Message[]>(
      this.baseUrl + 'messages/thread/' + username
    );
  }
  async sendMessage(username: string, content: string) {
    return this.hubConnection
      ?.invoke('SendMessage', { recipientUsername: username, content })
      .catch((e) => console.log(e));
  }
  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((e) => console.log(e));

    this.hubConnection.on('ReceiveMessageThread', (messages) => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some((x) => x.username === otherUsername)) {
        this.messageThread$.pipe(take(1)).subscribe({
          next: (messages) => {
            messages.forEach((message) => {
              if (!message.dateRead) {
                message.dateRead = new Date(Date.now());
              }
            });
            this.messageThreadSource.next([...messages]);
          },
        });
      }
    });

    this.hubConnection.on('NewMessage', (message) => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: (messages) => {
          this.messageThreadSource.next([...messages, message]);
        },
      });
    });
  }
  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop().catch((e) => console.log(e));
    }
  }
}
