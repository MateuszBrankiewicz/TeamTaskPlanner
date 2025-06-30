import { Component, input, Input } from '@angular/core';

@Component({
  selector: 'app-single-list-item',
  imports: [],
  templateUrl: './single-list-item.component.html',
  styleUrl: './single-list-item.component.scss'
})
export class SingleListItemComponent {
  @Input()
  numberOfMemebers: string = "";
  @Input()
  priority: string = '';
  @Input()
  dueDate: string = '';
  @Input()
  completedTaskCount: string ='';
  @Input()
  taskCount: string="";
  @Input()
  description: string ='';
  @Input()
  title: string = "";

}
