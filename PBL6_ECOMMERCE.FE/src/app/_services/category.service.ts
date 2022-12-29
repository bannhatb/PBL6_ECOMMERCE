import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';

const API =  'https://localhost:7220'
const LIST_CATEGORY = API + '/api/Category/list-category';
const ADD_CATEGORY_URL = API + '/api/Category/add-category';
const UPDATE_CATEGORY_URL = API + '/api/Category/update-category'
const DELETE_CATEGORY_BY_ID_URL = (id:any) => API + '/api/Category/delete-category/' + id;
const GET_CATEGORY_BY_ID = (id:any) => API + '/api/Category/get-category-by/' + id;


@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(
    private httpClient: HttpClient,
    private bussiness: BusinessService
    ) { }

  getCategories(){
    return this.httpClient.get(LIST_CATEGORY, this.bussiness.getRequestOptions())
  }
  createCategory(data:any){
    return this.httpClient.post(ADD_CATEGORY_URL, JSON.stringify(data), this.bussiness.getRequestOptions())
  }

  getCategoryById(id:any){
    return this.httpClient.get(GET_CATEGORY_BY_ID(id), this.bussiness.getRequestOptions())
  }

  updateCategory(data:any){
    return this.httpClient.put(UPDATE_CATEGORY_URL, JSON.stringify(data), this.bussiness.getRequestOptions())
  }

  deleteCategory(id:any){
    return this.httpClient.delete(DELETE_CATEGORY_BY_ID_URL(id), this.bussiness.getRequestOptions())
  }

}
