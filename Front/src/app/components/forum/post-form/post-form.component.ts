import { Component, EventEmitter, Input, output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorComponent } from '../../shared/editor/editor.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-post-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    EditorComponent,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './post-form.component.html',
  styleUrl: './post-form.component.scss'
})
export class PostFormComponent {
  @Input() postForm!: FormGroup;
  cancel = output<void>();
  submitPost = output<void>();

  constructor(private fb: FormBuilder) { }

  getContent(): FormControl {
    return this.postForm.get('content') as FormControl;
  }

  createPost() {
    if (this.postForm.valid) {
      this.submitPost.emit();
    }
  }

  onCancel() {
    this.cancel.emit();
  }
}
