import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PostService } from '../../services/post.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Comment } from '../../models/comment';

@Component({
  selector: 'app-comments',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.scss'
})
export class CommentsComponent {
  @Input() postId: number = 0;
  comments: Comment[] = [];
  commentForm: FormGroup;

  constructor(private fb: FormBuilder, private postService: PostService) {
    this.commentForm = this.fb.group({
      content: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadComments();
  }

  loadComments() {
    this.postService.getComments(this.postId).subscribe(comments => {
      this.comments = comments;
    });
  }

  addComment() {
    if (this.commentForm.valid) {
      const newComment: Comment = {
        postId: this.postId,
        content: this.commentForm.value.content
      };

      // this.postService.addComment(newComment).subscribe(comment => {
        this.comments.push(newComment);  // הוספת התגובה לרשימה
        this.commentForm.reset();
      // });
    }
  }
}
