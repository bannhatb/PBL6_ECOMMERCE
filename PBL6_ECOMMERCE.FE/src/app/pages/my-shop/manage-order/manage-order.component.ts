
import { Component, EventEmitter, Input, OnInit, Output ,ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShoporderdetailService } from 'src/app/_services/shoporderdetail.service';


@Component({
  selector: 'app-manage-order',
  templateUrl: './manage-order.component.html',
  styleUrls: ['./manage-order.component.css'],
  encapsulation: ViewEncapsulation.None,
  styles: [
		`
			.dark-modal .modal-content {
				background-color: #292b2c;
				color: white;
			}
			.dark-modal .close {
				color: white;
			}
			.light-blue-backdrop {
				background-color: #5cb3fd;
			}
		`,
  ]
})
export class ManageOrderComponent implements OnInit {
  @Input() orderdetails: any;
  orderdetailsData :any;
  listOrderDetails : any;
  closeResult: string;
  mode: any;
  pageSize =10;
  page = 1 ;
  constructor(
    private modalService: NgbModal,
    private shopOrderDetailService: ShoporderdetailService ) { }

  ngOnInit(): void {
    this.loadOrderDetails();
  }
  modeOpen(id : any){

  }
  handlePageChange(event : any) {
    this.page = event;
  }
  show_detail(content:any, id: any){
    console.log(id);
    console.log("HULLO")
    this.modalService.open(content, { size: 'lg' });
  }
  loadOrderDetails(){
    this.shopOrderDetailService.getOrderDetails()
        .subscribe((res) => this.handleGetOrderDetailsSuccess(res),
        (err) => this.handleGetOrderDetailsError(err))
  }
  handleGetOrderDetailsError(err: any){
    console.log(err);
  }
  handleGetOrderDetailsSuccess(res: any){
    this.orderdetails = res.result.data
    let dataOrder = this.orderdetails
    this.orderdetailsData = dataOrder
    console.log(res)
  }

  viewall(){
    this.orderdetailsData = this.orderdetails
  }
  viewstock(){
    this.orderdetailsData = this.orderdetails.filter((x:any) => x.sate == 1)
  }
  viewoutofstock(){
    this.orderdetailsData = this.orderdetails.filter((x:any) => x.sate == 2)
  }
}

