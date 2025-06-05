import {Component, EventEmitter, Input} from '@angular/core';

@Component({
  selector: 'app-button',
  imports: [],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent {
  buttonClick = new EventEmitter();
onClickEvent(){
  this.buttonClick.emit();
}
 @Input()
  text:string = '';
}
