import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorComponent } from '../editor/editor.component';

@Component({
  selector: 'app-post-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    EditorComponent,
    MatFormFieldModule,
    MatInputModule,
    ],
  templateUrl: './post-form.component.html',
  styleUrl: './post-form.component.scss'
})
export class PostFormComponent {
  @Input() postForm!: FormGroup;  
  @Input() showPostForm!: boolean;
  @Output() toggleForm = new EventEmitter<void>(); 
  @Output() submitPost = new EventEmitter<void>(); 

  constructor(private fb: FormBuilder) {}

  getContent(): FormControl {
    return this.postForm.get('content') as FormControl;
}
  createPost() {
    if (this.postForm.valid) {
      this.submitPost.emit(); 
    }
  }

  onCancel() {
    this.toggleForm.emit(); 
  }
}
