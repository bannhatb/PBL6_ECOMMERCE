import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewListProductComponent } from './view-list-product.component';

describe('ViewListProductComponent', () => {
  let component: ViewListProductComponent;
  let fixture: ComponentFixture<ViewListProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewListProductComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewListProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
