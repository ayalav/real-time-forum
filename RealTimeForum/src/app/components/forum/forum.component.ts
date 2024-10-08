import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { PostService } from '../../services/post.service';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import {
  ClassicEditor,
  Bold,
  Essentials,
  Heading,
  Indent,
  IndentBlock,
  Italic,
  Link,
  List,
  MediaEmbed,
  Paragraph,
  Table,
  Undo
} from 'ckeditor5';
import 'ckeditor5/ckeditor5.css';

@Component({
  selector: 'app-forum',
  standalone: true,
  imports: [
    CKEditorModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './forum.component.html',
  styleUrl: './forum.component.scss'
})
export class ForumComponent implements OnInit {
  posts: any[] = [];
  postForm: FormGroup;  
  showPostForm = false;
  public Editor = ClassicEditor;
  public config = {
    toolbar: [
      'undo', 'redo', '|',
      'heading', '|', 'bold', 'italic', '|',
      'link', 'insertTable', 'mediaEmbed', '|',
      'bulletedList', 'numberedList', 'indent', 'outdent'
    ],
    height: 300,
    plugins: [
      Bold,
      Essentials,
      Heading,
      Indent,
      IndentBlock,
      Italic,
      Link,
      List,
      MediaEmbed,
      Paragraph,
      Table,
      Undo
    ]
  }

  constructor(private fb: FormBuilder, private postService: PostService) {
    // Initialize form
    this.postForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required]
    });
  }

  ngOnInit(): void {   
    this.loadPosts();
  }

  loadPosts() {
    this.postService.getPosts().subscribe(posts => {
      this.posts = posts;
    });
  }

  createPost() {
    if (this.postForm.valid) {
      const newPost = this.postForm.value;
      // this.postService.createPost(newPost).subscribe(post => {
        this.posts.push(newPost);
        this.postForm.reset();
        this.showPostForm = false;
      // });
    }
  }

  togglePostForm() {
    this.showPostForm = !this.showPostForm;
  }

  toggleComments(post: any) {
    post.showComments = !post.showComments;
  }
}
