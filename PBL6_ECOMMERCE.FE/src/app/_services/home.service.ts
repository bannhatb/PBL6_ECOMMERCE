import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';

const API =  'https://localhost:7220';
const GET_ALL_PRODUCT = API +  '/api/Home/get-list-product';
const GET_ALL_PRODUCTDETAIL = (id:any) => API +   '/api/Home/get-product-detail-by/'  + id;
const GET_IMG_PRODUCTDETAIL = (id:any) => API +   '/api/Home/get-image-by-product-detail-id/'  + id;
const SEARCH_PROUCT = (key:any) => API +   '/api/Home/search-product-by/'  + key;
const GET_PRODUCT_BY_SHOP_ID = API +  '/api/Home/get-list-product-of-shop-by/2';



@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(
    private httpClient: HttpClient,
    private bussiness: BusinessService
  ) { }

  getAllProduct(){
    console.log("daica123");
    return this.httpClient.get(GET_ALL_PRODUCT, this.bussiness.getRequestOptions())
  }
  getAllProductDetail(id:any){
    return this.httpClient.get(GET_ALL_PRODUCTDETAIL(id), this.bussiness.getRequestOptions())
  }
  getImgProductDetail(id:any){
    return this.httpClient.get(GET_IMG_PRODUCTDETAIL(id), this.bussiness.getRequestOptions())
  }
  getPdByShopId(){
    return this.httpClient.get(GET_PRODUCT_BY_SHOP_ID, this.bussiness.getRequestOptions())
  }
  search(key:any){
    return this.httpClient.get(SEARCH_PROUCT(key), this.bussiness.getRequestOptions())
  }
}
