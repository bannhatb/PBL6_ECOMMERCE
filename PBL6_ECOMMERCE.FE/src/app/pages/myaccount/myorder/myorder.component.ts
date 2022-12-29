import { Component, OnInit, Input} from '@angular/core';
import { OrderService } from 'src/app/_services/order.service';

@Component({
  selector: 'app-myorder',
  templateUrl: './myorder.component.html',
  styleUrls: ['./myorder.component.css']
})
export class MyorderComponent implements OnInit {

  @Input()order :any
  currentDate = new Date();
  selectStyle : string;
  sentStyle :string;
  confirmStyle :string;
  deliveringStyle :string;
  receivedStyle :string;
  cancalledStyle:string;

  constructor(
    private orderService : OrderService
  ) { }


  ngOnInit(): void {
    this.loadSelection();
    this.loadOrder();
  }

  loadOrder(){
    this.orderService.getAllUserOrder()
    .subscribe(
      (res) => this.handleGetOrderSuccess(res),
      (err) => this.handleGetOrderError(err)
    )
  }

  handleGetOrderError(err: any){
    console.log(err)
  }
  handleGetOrderSuccess(res: any){
    this.order = res.result.data
    console.log(res)
  }
  loadSelection(){
    this.selectStyle ="default";
    this.sentStyle = "sent";
    this.confirmStyle = "default";
    this.deliveringStyle = "default";
    this.receivedStyle = "default";
    this.cancalledStyle = "default";
  }
  onSelect(select:any){
    if(select == 'sent'){
      this.sentStyle = 'sent'
      this.confirmStyle = "default";
      this.deliveringStyle = "default";
      this.receivedStyle = "default";
      this.cancalledStyle = "default";
    }
    else if(select == 'confirm'){
      this.sentStyle = "default";
      this.confirmStyle = "confirm";
      this.deliveringStyle = "default";
      this.receivedStyle = "default";
      this.cancalledStyle = "default";
      this.order = "";
    }
    else if(select == 'delivering'){
      this.sentStyle = "default";
      this.confirmStyle = "default";
      this.deliveringStyle = "delivering";
      this.receivedStyle = "default";
      this.cancalledStyle = "default";
    }
    else if(select == 'received'){
      this.sentStyle = "default";
      this.confirmStyle = "default";
      this.deliveringStyle = "default";
      this.receivedStyle = "received";
      this.cancalledStyle = "default";
    }
    else{
      this.sentStyle = "default";
      this.confirmStyle = "default";
      this.deliveringStyle = "default";
      this.receivedStyle = "default";
      this.cancalledStyle = "cancalled";
    }
  }
}
