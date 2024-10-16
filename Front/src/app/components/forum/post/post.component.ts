import { Component, input, Input } from '@angular/core';
import { Post } from '../../../models/post';
import { CommentsComponent } from '../../forum/comment/comments.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-post',
  standalone: true,
  imports: [
    CommentsComponent,
    MatButtonModule
  ],
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  post = input.required<Post>();

  toggleComments() {
    const currentPost = this.post(); 
    currentPost.showComments = !currentPost.showComments;
  }
}