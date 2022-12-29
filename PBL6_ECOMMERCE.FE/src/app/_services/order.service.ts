import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';
import { HttpClient } from '@angular/common/http';

const API =  'https://localhost:7220';
const ADD_ORDER = API + '/api/Order/add-order';
const GET_ORDER_BY_ID = (id:any) => API + '/api/Order/View-order?orderId=' + id;
const GET_ALL_ORDER_USER = API + '/api/Order/view-order-of-user';
@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(
    private httpClient: HttpClient,
    private bussiness: BusinessService
  ) { }

  addOrder(data : any){
    console.log(data)
    return this.httpClient.post(ADD_ORDER, JSON.stringify(data), this.bussiness.getRequestOptions())
  }

  getOrder(id: any){
    console.log(id);
    return this.httpClient.get(GET_ORDER_BY_ID(id), this.bussiness.getRequestOptions())
  }
  getAllUserOrder(){
    return this.httpClient.get(GET_ALL_ORDER_USER , this.bussiness.getRequestOptions())
  }
}
