import { Component, Input } from '@angular/core';
import { Post } from '../../../models/post';
import { CommentsComponent } from '../../forum/comment/comments.component';

@Component({
  selector: 'app-post',
  standalone: true,
  imports: [
    CommentsComponent
  ],
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  @Input() post!: Post; 
  showComments = false;

  toggleComments() {
    this.showComments = !this.showComments;
  }
}