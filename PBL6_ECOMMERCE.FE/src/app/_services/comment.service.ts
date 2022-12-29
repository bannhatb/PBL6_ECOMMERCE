import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusinessService } from './business.service';

const API =  'https://localhost:7220'
const GET_COMMENT_PRODUCT_ID = (id:any) => API + '/api/Comment/list-comment-by/1?productId=' + id;

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(
    private httpClient: HttpClient,
    private bussiness: BusinessService
  ) { }

  getCommentOfProduct(id : any){
    return this.httpClient.get(GET_COMMENT_PRODUCT_ID(id), this.bussiness.getRequestOptions())
  }
}
