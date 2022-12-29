import { Component, EventEmitter, Input, OnInit, Output ,ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductService } from 'src/app/_services/product.service';


@Component({
  selector: 'app-revenue',
  templateUrl: './revenue.component.html',
  styleUrls: ['./revenue.component.css']
})
export class RevenueComponent implements OnInit {
  @Input() productdetails: any;
  constructor( 
      private modalService: NgbModal,
      private productservice: ProductService) { }
  
      
  ngOnInit(): void {
    this.loadRevenueProduct();
  }
  handleGetOrderDetailsError(err: any){
    console.log(err);
    console.log("thinhnguyen1233456");
  }
  handleGetOrderDetailsSuccess(res: any){
    
    this.productdetails = res.result.data;
    
    console.log(res)
  }
  loadRevenueProduct(){
    this.productservice.statisticProduct()
        .subscribe((res) => this.handleGetOrderDetailsSuccess(res),
        (err) => this.handleGetOrderDetailsError(err))
  }
}
