import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleListItemComponent } from './single-list-item.component';

describe('SingleListItemComponent', () => {
  let component: SingleListItemComponent;
  let fixture: ComponentFixture<SingleListItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SingleListItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SingleListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
