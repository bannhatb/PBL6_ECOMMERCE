import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';



const API =  'https://localhost:7220'
const LIST_VOUCHER = API + '/api/Shop/get-voucher-of-shop';
const ADD_CATEGORY_URL = API + '/api/Category/add-category';
const UPDATE_CATEGORY_URL = API + '/api/Category/update-category'
const DELETE_CATEGORY_BY_ID_URL = (id:any) => API + '/api/Category/delete-category/' + id;
const GET_CATEGORY_BY_ID = (id:any) => API + '/api/Category/get-category-by/' + id;


@Injectable({
  providedIn: 'root'
})
export class ShopService {

  constructor(
    private httpClient: HttpClient,
    private bussiness: BusinessService
    ) { }

  getAllVoucherShop(){
    return this.httpClient.get(LIST_VOUCHER, this.bussiness.getRequestOptions())
  }

}
