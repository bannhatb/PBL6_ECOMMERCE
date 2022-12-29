import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CategoryService } from 'src/app/_services/category.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  @Input() category:any
  @Output() deleteCategoryEvent = new EventEmitter<number>()
  constructor(
    private categoriesService: CategoryService
  ) { }

  ngOnInit(): void {
  }

  onDeletedCategory(){
    if(window.confirm("Ban thuc su muon xoa")){
      this.categoriesService.deleteCategory(this.category.id)
      .subscribe(
        (res:any) => {
          this.deleteCategoryEvent.emit(this.category.id)
        },
        (err) => {
          alert("Delete fail. Detail: " + JSON.stringify(err))
        }
      )
    }
  }
}
