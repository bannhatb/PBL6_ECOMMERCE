import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';

const API =  'https://localhost:7220'
const GET_VOUCHER_AVAIABLE = API + '/api/Voucher/get-voucher-availability' ;
const GET_VOUCHER_SHOP_BY_ID = (id:any) => API + '/api/Voucher/get-voucher-product-by/' + id;


@Injectable({
  providedIn: 'root'
})
export class VoucherService {

  constructor(
    private httpClient: HttpClient,
    private bussiness: BusinessService
  ) { }

  getVoucherAvaiable(){
    return this.httpClient.get(GET_VOUCHER_AVAIABLE , this.bussiness.getRequestOptions())
  }
  getVoucherShopById(id:any){
    return this.httpClient.get(GET_VOUCHER_SHOP_BY_ID(id) , this.bussiness.getRequestOptions())
  }
}
