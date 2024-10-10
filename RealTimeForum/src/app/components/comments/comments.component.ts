import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PostService } from '../../services/post.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { Comment } from '../../models/comment';
import { SignalRService } from '../../services/signalR.service';

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

  constructor(
    private fb: FormBuilder,
    private postService: PostService,
    private signalRService: SignalRService,
    private snackBar: MatSnackBar
  ) {
    this.commentForm = this.fb.group({
      content: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadComments();

    this.signalRService.addCommentUpdateListener();

    this.signalRService.comments$.subscribe(newCommentMessage => {
      if (newCommentMessage) {
        this.loadComments();
      }
    });
  }

  loadComments() {
    this.postService.getComments(this.postId).subscribe(comments => {
      this.comments = comments;
    });
  }

  addComment() {
    if (this.commentForm.valid) {
      const newComment: Comment = {
        content: this.commentForm.value.content
      };

      this.postService.addComment(this.postId, newComment).subscribe(
        comment => {
          this.comments.push(comment);
          this.commentForm.reset();
          this.snackBar.open('Comment added successfully!', 'Close', { duration: 3000 });
        },
        error => {
          this.snackBar.open('Failed to add comment. Please try again.', 'Close', { duration: 3000 });
        }
      );
    }
  }
}
