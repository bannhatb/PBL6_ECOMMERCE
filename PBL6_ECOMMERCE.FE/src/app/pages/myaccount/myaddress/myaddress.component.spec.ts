import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyaddressComponent } from './myaddress.component';

describe('MyaddressComponent', () => {
  let component: MyaddressComponent;
  let fixture: ComponentFixture<MyaddressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyaddressComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyaddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
