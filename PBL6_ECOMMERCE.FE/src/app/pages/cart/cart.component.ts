import { Component, EventEmitter, Input, OnInit, Output ,ViewEncapsulation } from '@angular/core';
import { IsActiveMatchOptions } from '@angular/router';
import { CartService } from 'src/app/_services/cart.service';
import { VoucherService } from 'src/app/_services/voucher.service';
import { Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShopService } from 'src/app/_services/shop.service';


@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class CartComponent implements OnInit {
  @Input() cart:any
  @Output() deletecartEvent = new EventEmitter<number>()
  @Input() vouchers:any
  @Input() orderVouchers:any
  data = {id : "1", username : "thinh"}
  constructor(
    private cartService : CartService,
    private voucherService : VoucherService,
    private router: Router,
    private modalService: NgbModal,
    private shopService :ShopService
    // private transfereService :TransfereService
  ) { }

	openModalShopVoucher(content: any,idShop:any) {
		this.modalService.open(content, { centered: true });
    this.loadVoucherShop(idShop)
	}
  openModalOrderVoucher(content: any) {
		this.modalService.open(content, { centered: true });
    this.loadVoucherOrder()
	}
  order : Array<any> =[];
  sumPrice : number = 0;
  isChecked : Array<any>[] = []
  // arrCheck : Array<Array<number>>
  arrCheck : Array<any>[]= []
  arrShopId: Array<number> = [];
  listPdShop : Array<any>[]=[];
  showingVoucher = false
  ngOnInit(): void {
    this.loadCart();

    // this.loadVoucherOrder()
    console.log(this.orderVouchers);

  }
  loadCart(){
    this.cartService.getCart()
    .subscribe(
      (res) => this.handleGetCartSuccess(res),
      (err) => this.handleGetCartError(err)
    )
  }

  handleGetCartError(err: any){
    console.log(err);
  }
  handleGetCartSuccess(res: any){
    this.cart = res.result.data
    console.log(res)
    this.loadPdShop()
  }

  loadPdShop(){
    for (var i = 0; i<this.cart.length; i++){
      this.arrShopId.push(this.cart[i].idShop)
      console.log();
    }
    this.arrShopId = this.arrShopId.filter((value,index) => this.arrShopId.indexOf(value) === index)
    console.log(this.arrShopId);
    for(var i = 0; i<this.arrShopId.length; i++){
      this.listPdShop.push(this.cart.filter((x:any) => {return this.arrShopId[i] === x.idShop}))
    }
    this.checkMatrix()
  }

  checkMatrix(){

    for(var j =0 ; j < this.arrShopId.length ; j++){
      let arrCheckValue =[]
      for (var i = 0; i < 30 ; i++){
        arrCheckValue.push(false)
      }
      this.isChecked.push(arrCheckValue)
    }
    console.log(this.isChecked)
  }

  checkValue(i:any,j:any,pd :any){
      if (this.isChecked[i][j] ==  true){
      this.order.push(pd)
      console.log(this.order)
      this.sumPrice += pd.price * pd.amount;
    }
    else{
      this.order = this.order.filter(item => item !== pd)
      this.sumPrice -= pd.price * pd.amount;
    }

  }

  deleteItem(pd : any,i:any){
    if(window.confirm("Ban thuc su muon xoa")){
      this.cartService.deleteItem(pd.id)
      .subscribe(
        (res:any) => {
          this.deletecartEvent.emit(pd.id)
          this.cart = this.cart.filter((item:any) => item !==pd)
          this.listPdShop[i] = this.listPdShop[i].filter((item:any)=> item !== pd )
          this.sumPrice -= pd.price * pd.amount;
        },
        (err) => {
          alert("Delete fail. Detail: " + JSON.stringify(err))
        }
      )
    }
    this.order = this.order.filter(item => item !== pd)
    // this.cartService.deleteItem(pd.id)
  }

  decreaseQty(pd : any, i: any,j:any){

    if (pd.amount > 1){
      pd.amount -= 1;
      if(this.isChecked[i] ){
        this.order = this.order.filter(item => item !== pd)
        this.sumPrice -= pd.price
        this.order.push(pd)
      }
    }
  }

  increaseQty(pd : any, i: any,j:any){
    pd.amount += 1;
    if(this.isChecked[i][j] ){
      this.order = this.order.filter(item => item !== pd)
      this.sumPrice += pd.price
      this.order.push(pd)
    }
  }


  loadVoucherShop(id:any){
    this.voucherService.getVoucherShopById(id)
    .subscribe(
      (res) => this.handleGetVoucherSuccess(res),
      (err) => this.handleGetVoucherError(err)
    )
  }
  handleGetVoucherError(err: any){
    console.log(err)
  }
  handleGetVoucherSuccess(res: any){
    this.vouchers = res.result.data
    console.log(this.arrShopId);
    console.log(res)
  }

  loadVoucherOrder(){
    this.voucherService.getVoucherAvaiable()
    .subscribe(
      (res) => this.handleGetOrderVoucherSuccess(res),
      (err) => this.handleGetOrderVoucherError(err)
    )
  }
  handleGetOrderVoucherError(err: any){
    console.log(err)
  }
  handleGetOrderVoucherSuccess(res: any){
    this.orderVouchers = res.result.data
    console.log(this.orderVouchers);

  }
  check(order: any){
    console.log(order);
    let data = JSON.stringify(order);
    this.router.navigate(['/order'], { queryParams:  {data} ,  skipLocationChange: true });
    // this.router.createUrlTree(['/order', {my_order: JSON.stringify(order)}]);

  }

}



