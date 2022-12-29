import { Component, Input, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/_services/category.service';
import { ProductService } from 'src/app/_services/product.service';


@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {
  constructor(
    private categoryService : CategoryService,
    private productService : ProductService,
  ) { }
  public expand = false;
  @Input() categories :any
  namePd : string
  categoryPd : string
  listPdDetail :Array<any> =[]
  description : string
  avtUrl :any;
  sizePd : any
  colorPd : any
  amountPd: number
  pricePd : number
  initialPricePd :number
  ngOnInit(): void {
    this.loadCategory()
  }

  loadCategory(){
    this.categoryService.getCategories()
    .subscribe(
      (res) => this.handleGetCategoriesSuccess(res),
      (err) => this.handleGetCategoriesError(err)
    )
  }

  handleGetCategoriesError(err: any){
    console.log(err);
  }
  handleGetCategoriesSuccess(res: any){
    this.categories = res.result.data
    this.categoryPd = this.categories[0];
  }
  addProduct() {
    const submitData = {
      "id": 0,
      "name": this.namePd ,
      "material": "string",
      "origin": "string",
      "description": "string",
      "status": true,
      "categories": [
        parseInt(this.categoryPd)

      ]
    }
    console.log(submitData);
    this.productService.addProduct(submitData)
    .subscribe(
      (res) => this.handleAddPdSuccess(res),
      (err) => this.handleAddPdError(err)
    )
    for (let i =0 ; i <this.listPdDetail.length ; i++){
      console.log(this.listPdDetail[i]);
      const submitDataDetail = {
        "id": 0,
        "productId": 36,
        "size": this.listPdDetail[i].size,
        "color": this.listPdDetail[i].color,
        "amount": parseInt(this.listPdDetail[i].amount),
        "price": parseInt(this.listPdDetail[i].price),
        "initialPrice": parseInt(this.listPdDetail[i].initialPrice),
      }
      const submitImg = {
        "id": 0,
        "productDetailId": 36,
        "urlImage": this.listPdDetail[i].urlImg
      }
      console.log(submitDataDetail);

      this.productService.addImgPd(submitImg)
    .subscribe(
      (res) => this.handleAddPdDetailSuccess(res),
      (err) => this.handleAddPdDetailError(err)
    )
      this.productService.addProducDetail(submitDataDetail)
    .subscribe(
      (res) => this.handleAddPdDetailSuccess(res),
      (err) => this.handleAddPdDetailError(err)
    )
    }
    this.listPdDetail = []

  }
  handleAddPdError(err: any){
    console.log(err);
  }
  handleAddPdSuccess(res: any){
    alert("them san pham thanh cong")
  }
  handleAddPdDetailError(err: any){
    console.log(err);
  }
  handleAddPdDetailSuccess(res: any){
    console.log(res)
  }

  typeNamePd(e:any){
    this.namePd = e.target.value
  }
  categorySelected(e:any){
    this.categoryPd = e.target.value
    console.log(e.target.value);

  }

  sizeInput(e:any){
    this.sizePd = e.target.value
  }
  colorInput(e:any){
    this.colorPd = e.target.value
  }
  amountInput(e:any){
    this.amountPd = e.target.value
  }
  initialPriceInput(e:any){
    this.initialPricePd = e.target.value
  }
  priceInput(e:any){
    this.pricePd = e.target.value
  }
  addPdDetail(){
    type pdDetail1 = {
      size : string | undefined;
      color : string | undefined;
      amount:number;
      price :number;
      initialPrice :number;
      productId: number;
      urlImg : string;
    }
    if( this.colorPd === undefined)
    {
      this.colorPd = 0;
    }
    if( this.sizePd === undefined)
    {
      this.sizePd = 0;
    }
    let pdDetail = <pdDetail1>{}
    console.log(pdDetail);
    pdDetail.size = this.sizePd;
    pdDetail.color = this.colorPd
    pdDetail.amount = this.amountPd
    pdDetail.initialPrice = this.initialPricePd
    pdDetail.price = this.pricePd
    pdDetail.urlImg = this.avtUrl
    pdDetail.productId = 29
    this.listPdDetail.push(pdDetail)
    console.log(this.listPdDetail);

  }
  onSelectFile(e:any){
    if(e.target.files){
      var reader = new FileReader();
      reader.readAsDataURL(e.target.files[0]);
      reader.onload=(event:any)=>{
        this.avtUrl=event.target.result;
        console.log(event.target.result);

      }
    }
  }

}
