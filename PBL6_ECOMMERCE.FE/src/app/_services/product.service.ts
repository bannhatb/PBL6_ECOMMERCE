import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';

const API =  'https://localhost:7220'
const ADD_PRODUCT_URL = API + '/api/Product/add-product';
const UPDATE_PRODUCT_URL = API + '/api/Product/update-product'
const DELETE_PRODUCT_BY_ID_URL = (id:any) => API + '/api/Product/delete-product/' + id;
const GET_PRODUCT_BY_ID = (id:any) => API + '/api/Product/get-product-by/' + id;
const STATISTIC_PRODUCT = API + '/api/Statistic/statistic-product'
const ADD_PRODUCT = API + '/api/Product/add-product'
const ADD_PRODUCT_DETAIL = API + '/api/Product/add-product-detail'
const ADD_IMG_PD = API + '/api/Product/add-product-image'


@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(
    private httpClient: HttpClient,
    private bussiness: BusinessService
  ) { }


  createCategory(data:any){
    return this.httpClient.post(ADD_PRODUCT_URL, JSON.stringify(data), this.bussiness.getRequestOptions())
  }

  getCategoryById(id:any){
    return this.httpClient.get(GET_PRODUCT_BY_ID(id), this.bussiness.getRequestOptions())
  }

  updateCategory(data:any){
    return this.httpClient.put(UPDATE_PRODUCT_URL, JSON.stringify(data), this.bussiness.getRequestOptions())
  }

  deleteCategory(id:any){
    return this.httpClient.delete(DELETE_PRODUCT_BY_ID_URL(id), this.bussiness.getRequestOptions())
  }
  statisticProduct(){
    return this.httpClient.get(STATISTIC_PRODUCT, this.bussiness.getRequestOptions())

  }

  addProduct(data:any){
    return this.httpClient.post(ADD_PRODUCT, JSON.stringify(data), this.bussiness.getRequestOptions())
  }
  addProducDetail(data:any){
    return this.httpClient.post(ADD_PRODUCT_DETAIL, JSON.stringify(data), this.bussiness.getRequestOptions())
  }
  addImgPd(data:any){
    return this.httpClient.post(ADD_PRODUCT_DETAIL, JSON.stringify(data), this.bussiness.getRequestOptions())

  }
}
