import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-post-form',
  standalone: true,
  imports: [],
  templateUrl: './post-form.component.html',
  styleUrl: './post-form.component.scss'
})
export class PostFormComponent {
  @Input() postForm!: FormGroup;
  @Input() showPostForm!: boolean;
  @Output() toggleForm = new EventEmitter<void>();
  @Output() submitPost = new EventEmitter<void>();

  constructor(private fb: FormBuilder) { }

  // Getter for content FormControl
  getContent() {
    return this.postForm.get('content');
  }

  onSubmit() {
    if (this.postForm.valid) {
      this.submitPost.emit();
    }
  }

  onCancel() {
    this.toggleForm.emit();
  }
}
