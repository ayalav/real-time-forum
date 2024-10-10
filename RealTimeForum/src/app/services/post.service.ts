import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from '../models/post';
import { Comment } from '../models/comment';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  private apiUrl = 'https://localhost:7260/post';

  constructor(private http: HttpClient) {}

  // Fetch all posts
  getPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.apiUrl}`);
  }

  // Create a new post 
  createPost(post: Post): Observable<Post> {
    return this.http.post<Post>(`${this.apiUrl}`, post);
  }

   // Fetch comments for a specific post
  getComments(postId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/${postId}/comments`);
  }

    // Add a comment to a specific post
    addComment(postId: number, comment: Comment): Observable<Comment> {
      return this.http.post<Comment>(`${this.apiUrl}/${postId}/comments`, comment);
  }
}
