import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-button',
  imports: [],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent {
  @Output()
  buttonClick = new EventEmitter();
onClickEvent(){
  this.buttonClick.emit();
}
 @Input()
 type: string = "button";
 @Input()
  text:string = '';
@Input()
  size: 'small' | 'medium' | 'large' = 'large';
}
