import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { PostService } from '../../services/post.service';
import { Post } from '../../models/post';
import { SignalRService } from '../../services/signalR.service';
import { PostComponent } from './post/post.component';
import { PostFormComponent } from './post-form/post-form.component';

@Component({
  selector: 'app-forum',
  standalone: true,
  imports: [
    PostComponent,
    PostFormComponent,    
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './forum.component.html',
  styleUrl: './forum.component.scss'
})
export class ForumComponent implements OnInit {
  posts: Post[] = [];
  postForm: FormGroup;
  showPostForm = false; 

  constructor(
    private fb: FormBuilder,
    private postService: PostService,
    private signalRService: SignalRService
  ) {
    // Initialize form
    this.postForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadPosts();
    this.signalRService.startConnection();
    this.signalRService.addPostUpdateListener();

    this.signalRService.posts$.subscribe(newPostMessage => {
      if (newPostMessage) {
        this.loadPosts();
      }
    });
  }

  loadPosts() {
    this.postService.getPosts().subscribe(posts => {
      this.posts = posts;
    });
  }

  createPost() {
    if (this.postForm.valid) {
      const newPost: Post = {
        id: 0,
        title: this.postForm.value.title,
        content: this.postForm.value.content,
        comments: []
      };
      this.postService.createPost(newPost).subscribe(post => {
        this.posts.push(newPost);
        this.postForm.reset();
        this.showPostForm = false;
      });
    }
  }

  togglePostForm() {
    this.showPostForm = !this.showPostForm;
  }
}
