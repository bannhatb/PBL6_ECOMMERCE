import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/_services/category.service';
import { Category } from 'src/app/_models/category';
import { ListCategory } from 'src/app/_models/category';
import { Data } from 'src/app/_models/category';
import { HomeService } from 'src/app/_services/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(
    private categoryService :CategoryService,
    private homeService :HomeService
  ) { }

  categories:Array<any> = []
  banners = [{id:1},{id:2}]
  arr : Array<Category> =new Array();
  data : Data =new Data();
  productData : any;

  ngOnInit(): void {
    this.loadHome();
    this.loadAllProduct();
  }

  loadHome(){
    this.categoryService.getCategories()
    .subscribe(
      (res) => this.handleGetCategorySuccess(res),
      (err) => this.handleGetCategoryError(err)
    )
  }

  handleGetCategoryError(err: any){
    console.log(err)
  }
  handleGetCategorySuccess(res: any){
    this.categories = res.result.data
    console.log(res)
  }
  onDeleteCategoryEvent(categoryId:number){
    console.log('Delete Category - ' + categoryId)
    this.loadHome()
  }

  loadAllProduct(){
    this.homeService.getAllProduct()
    .subscribe(
      (res) => this.handleGetProductSuccess(res),
      (err) => this.handleGetProductError(err)
    )
  }

  handleGetProductError(err: any){
    console.log(err)
  }

  handleGetProductSuccess(res: any){
    this.productData = res.result.data
    console.log(this.productData)
  }
}
