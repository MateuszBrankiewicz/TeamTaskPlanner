import {Component, Input} from '@angular/core';
import {FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-form-input',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './form-input.component.html',
  styleUrl: './form-input.component.scss'
})
export class FormInputComponent {

  @Input()
  placeholder: string ="";
  @Input()
  type: "text" | "password" | "email" | "number" = "text";
  @Input()
  control! : FormControl;
  @Input()
  size: 'small' | 'medium' | 'large' = 'large';
  @Input()
  label: string ="";
  @Input()
  id: string = crypto.randomUUID();

  @Input() submitted!: boolean;
}
