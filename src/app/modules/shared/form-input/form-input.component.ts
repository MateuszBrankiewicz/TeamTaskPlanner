import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-form-input',
  imports: [],
  templateUrl: './form-input.component.html',
  styleUrl: './form-input.component.scss'
})
export class FormInputComponent {

  @Input()
  placeholder: string ="";
  @Input()
  type: "text" | "password" | "email" | "number" = "text";
  @Input()
  value : string ="";
  @Input()
  size: 'small' | 'medium' | 'large' = 'large';
}
