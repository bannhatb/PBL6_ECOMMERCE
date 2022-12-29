import { Component, Input, OnInit } from '@angular/core';
import { HomeService } from 'src/app/_services/home.service';

@Component({
  selector: 'app-shop-view',
  templateUrl: './shop-view.component.html',
  styleUrls: ['./shop-view.component.css']
})
export class ShopViewComponent implements OnInit {


  @Input() products :any;
  constructor(
    private homeService :HomeService,
  ) { }

  ngOnInit(): void {
    this.loadPdShop()
  }
  loadPdShop(){
    this.homeService.getPdByShopId()
    .subscribe(
      (res) => this.handleGetPdSuccess(res),
      (err) => this.handleGetPdError(err)
    )
  }

  handleGetPdError(err: any){
    console.log(err);
    console.log("thinhnguyen1233456");
  }
  handleGetPdSuccess(res: any){
    this.products = res.result.data
  }
}
