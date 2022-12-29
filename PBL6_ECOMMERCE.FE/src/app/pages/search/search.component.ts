import { Component, OnInit,Input } from '@angular/core';
import { HomeService } from 'src/app/_services/home.service';
import { ActivatedRoute } from '@angular/router';



@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  constructor(
    private homeService : HomeService,
    private activatedRoute : ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.loadAllProduct()
    this.loadDataSearch()
    console.log(this.searchKey.data);

  }

  @Input() productData:any;
  pageSize =6;
  page = 1 ;
  key = 'price';
  reverse = false;
  category = "quần áo";
  searchKey :any
  searchKeyData :any
  handlePageChange(event : any) {
    this.page = event;
  }

  loadDataSearch(){
    this.activatedRoute.queryParams.subscribe(params => {
      let data = params['data']
      this.searchKey = params['data'];
      console.log(params);

    });
    this.searchKeyData =this.searchKey
  }
  onChange(deviceValue: any) {
    if (deviceValue === 'price-low-to-high'){
      this.reverse =false;
    }
    else{
      this.reverse = true
    }
  }
  loadAllProduct(){
    let keyData:string ='%C3%A1o';
    console.log(this.searchKey);
    console.log(this.searchKey == 'giày');

    if(this.searchKeyData == 'giày'){
      keyData = 'gi%C3%A0y'
    }
    if(this.searchKeyData == 'quần'){
      keyData = 'qu%E1%BA%A7n'
    }
    if(this.searchKeyData == 'dồng hồ'){
      keyData = 'gi%C3%A0y'
    }

    this.homeService.search(keyData)
    .subscribe(
      (res) => this.handleGetProductSuccess(res),
      (err) => this.handleGetProductError(err)
    )
  }

  handleGetProductError(err: any){
    console.log(err)
  }

  handleGetProductSuccess(res: any){
    this.productData = res.result.data
    console.log(this.productData)
  }
}
