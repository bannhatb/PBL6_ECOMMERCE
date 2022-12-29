import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';
import { HttpClient } from '@angular/common/http';

const API =  'https://localhost:7220'
const GET_ALL_ITEM_BY_USER_ID =  API + '/api/Cart/get-all-items-of-user';
const ADD_ITEM_TO_CART = API + '/api/Cart/Add-item-to-cart' ;
const UPDATE_ITEM_IN_CART = API + 'api/Cart/update-item-in-cart' ;
const GET_VNPAY_LINK = (id:any) => API + '/api/Cart/vnay-payment-link?orderId='+id+'&vnp_Returnurl=https%3A%2F%2Flocalhost%3A7220%2Fapi%2FCart%2Freturn-url'
const DELETE_ITEM_IN_CART = (id:any) => API + '/api/Cart/delete-item-in-cart/' + id;


@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(
    private httpClient: HttpClient,
    private businessService: BusinessService
  ) { }
  addItem2Cart(data: any){
    return this.httpClient.post(ADD_ITEM_TO_CART , JSON.stringify(data) , this.businessService.getRequestOptions())
  }
  updateItem(){
    return this.httpClient.post(UPDATE_ITEM_IN_CART, this.businessService.getRequestOptions())
  }
  deleteItem(id:any){
    return this.httpClient.delete(DELETE_ITEM_IN_CART(id), this.businessService.getRequestOptions())
  }
  getCart(){
    return this.httpClient.get(GET_ALL_ITEM_BY_USER_ID, this.businessService.getRequestOptions())
  }
  getVNPayLink(id:any){
    return this.httpClient.get(GET_VNPAY_LINK(id), this.businessService.getRequestOptions())
  }
}
