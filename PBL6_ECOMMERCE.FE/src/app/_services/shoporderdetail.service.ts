import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';

const API =  'https://localhost:7220'
const GET_ALL_ORDER_DETAIL_OF_SHOP = API + "/api/Shop/get-all-order-detail-of-shop"
@Injectable({
  providedIn: 'root'
})
export class ShoporderdetailService {

  constructor(
    private httpClient: HttpClient,
    private businessService: BusinessService
  ) { }
  getOrderDetails(){
    console.log("son")
    return this.httpClient.get(GET_ALL_ORDER_DETAIL_OF_SHOP, this.businessService.getRequestOptions())

  }

}
