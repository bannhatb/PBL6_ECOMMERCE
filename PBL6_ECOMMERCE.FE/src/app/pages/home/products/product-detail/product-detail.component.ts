import { Component, OnInit, Input } from '@angular/core';
import { ProductService } from 'src/app/_services/product.service';
import { HomeService } from 'src/app/_services/home.service';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CartService } from 'src/app/_services/cart.service';



@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {

  @Input() pdId : any;
  @Input() pdInfo : any;
  listPd : any;
  sizePd: Array<string> = [];
  colorPd: Array<string> = [];
  arrSize : string;
  arrProperty : Array<string>
  checkSize : Boolean;
  checkColor : Boolean;
  objPro = {}
  arrColorHave : Array<string>;
  form: FormGroup
  listPdDetail : any;
  selectedPd : any;
  oldSelectedColor = '';
  idPdSelected : number;
  productData:any;
  listImg : Array<string> = [];
  price = 0;
  initialPrice =0 ;
  overview= 'Sơ mi là một trong những loại phục trang có tính ứng dụng cao, bạn có thể sử dụng trong những bữa tiệc yêu cầu sự lịch lãm, hoặc những chuyến dạo chơi thoải mái bên bạn bè, hoặc ngay cả dịp hẹn hò cùng người yêu. Cùng Gavin tìm hiểu những tiêu chí khi chọn mua áo sơ mi sau đây.';
  qty=1;
  sum_price =this.price * this.qty;
  constructor(
    private  productService : ProductService,
    private activatedRoute : ActivatedRoute,
    private homeService : HomeService,
    private fb: FormBuilder,
    private cartService : CartService,
  ) { }

  ngOnInit(): void {

    this.makeForm()
    this.activatedRoute.paramMap.subscribe(params => {
    this.pdId = params.get('id');
    })
    this.loadPdDetail();
  }

  onSizeItemChange(e:any){
    interface IobjPro {
      [key: number]: Array<string>;
    }
    var objProReal :IobjPro
    objProReal  = this.objPro

    var targetSize = e.target.id
    this.arrColorHave = objProReal[targetSize]
    console.log(this.oldSelectedColor);

    if (!this.checkColor){
      this.selectedPd= this.listPdDetail.filter((x:any) => x.size == targetSize)
      let objPdDetail = this.listPd.filter((x:any) => x.id === this.selectedPd[0].id)
      let objPdDetailImg  = objPdDetail[0].productImages
      this.ImagePath =  objPdDetailImg[0].urlImage
      // this.loadImg(this.selectedPd[0].id)
      this.idPdSelected = this.selectedPd[0].id;
      this.price = this.selectedPd[0].price
      this.initialPrice = this.selectedPd[0].initialPrice
      this.sum_price =this.price * this.qty;
    }
    else{
      this.selectedPd= this.listPdDetail.filter((x:any) => x.size == targetSize)
      if(this.oldSelectedColor !== ''){
        var arrCheckOldColor = [];
        arrCheckOldColor = this.selectedPd.filter((x:any) => x.color == this.oldSelectedColor)
        if (arrCheckOldColor.length != 0){
          var newSelectedPd = this.selectedPd.filter((x:any) => x.color == this.oldSelectedColor)
          // let objPdDetail = this.listPd.filter((x:any) => x.id === newSelectedPd[0].id)
          // this.ImagePath = this.listPd[newSelectedPd[0].id].productImages[0].urlImage
          let objPdDetail = this.listPd.filter((x:any) => x.id === newSelectedPd[0].id)
          let objPdDetailImg  = objPdDetail[0].productImages
          this.ImagePath =  objPdDetailImg[0].urlImage
          // this.loadImg(newSelectedPd[0].id)
          this.idPdSelected = this.selectedPd[0].id;
          this.price = this.selectedPd[0].price
          this.initialPrice = this.selectedPd[0].initialPrice
          this.sum_price =this.price * this.qty;
        }
      }
      console.log(this.selectedPd);
    }
  }
  onColorItemChange(e:any){
    var targetColor = e.target.id
    this.oldSelectedColor = targetColor
    this.selectedPd= this.listPdDetail.filter((x:any) => x.color == targetColor)
    let objPdDetail = this.listPd.filter((x:any) => x.id === this.selectedPd[0].id)
    let objPdDetailImg  = objPdDetail[0].productImages
    this.ImagePath =  objPdDetailImg[0].urlImage
    // this.ImagePath = objPdDetail.productImages[0].urlImage
    // this.loadImg(this.selectedPd[0].id)
    this.idPdSelected = this.selectedPd[0].id;
    this.price = this.selectedPd[0].price
    this.initialPrice = this.selectedPd[0].initialPrice
    this.sum_price =this.price * this.qty;
  }

  loadPdDetail(){
    this.homeService.getAllProductDetail(this.pdId)
    .subscribe(
      (res) => this.handleGetAllProductDetailSuccess(res),
      (err) => this.handleGetAllProductDetailError(err)
    )
  }

  handleGetAllProductDetailError(err: any){
    console.log(err)
  }
  handleGetAllProductDetailSuccess(res: any){
    this.pdInfo = res.result.data
    this.listPd = res.result.data.productDetails
    console.log(res)
    this.loadProperty()
  }

  loadProperty(){
    for(let i=0; i < this.listPd.length; i++ ){
      this.sizePd.push(this.listPd[i].size)
      this.colorPd.push(this.listPd[i].color)
    }
    this.sizePd = this.sizePd.filter((value, index) => this.sizePd.indexOf(value) === index)
    this.colorPd = this.colorPd.filter((value, index) => this.colorPd.indexOf(value) === index)
    for(let i=0; i < this.sizePd.length; i++ ){
      this.arrSize = this.listPd.filter((x:any)=> { return x.size === this.sizePd[i]} ).map((x:any) => x.color)
      this.objPro ={...this.objPro ,[this.sizePd[i]] : this.arrSize }
    }
    this.listPdDetail = this.listPd;
    this.arrProperty = Object.getOwnPropertyNames(this.listPd[0])
    console.log();
    console.log(this.arrProperty.find(x => x === "size") !== null);

    if(this.listPd[0].size !== null){
      this.checkSize = true;
    }
    else{
      this.checkSize = false;
    }
    console.log(this.listPd[0].color);

    if(this.listPd[0].color !== null){
      this.checkColor =true
    }
    else{
      this.checkColor = false
    }
    if(this.checkSize == false){
      this.arrColorHave = this.colorPd
    }

    for (let i= 0 ; i < this.listPd.length ; i++){
      this.listImg.push(this.listPd[i].productImages[0].urlImage)
    }
    let firstImg = this.listImg[0];
    this.ImagePath = firstImg
    console.log(this.listImg);

    this.price = this.listPd[0].price
    this.initialPrice = this.listPd[0].initialPrice
    this.sum_price =this.price * this.qty;
  }
  makeForm(){
    this.form = this.fb.group({
      size: ['', Validators.required],
      color: ['', Validators.required],
    })
  }
  submitData(){
    const data = this.form.value
    console.log(data);
  }
  loadImg(id:any){
    this.homeService.getImgProductDetail(id)
    .subscribe(
      (res) => this.handleGetImgSuccess(res),
      (err) => this.handleGetImglError(err)
    )
  }

  handleGetImglError(err: any){
    console.log(err)
  }
  handleGetImgSuccess(res: any){
    this.ImagePath= res.result.data.urlImage
    console.log(res)
  }

  add2Cart(){
    console.log(this.qty);

    const submitData = {
      "productDetailId": this.idPdSelected,
      "amount": this.qty
    }
    this.cartService.addItem2Cart(submitData)
    .subscribe(
      (res:any) => {
        //return home
        console.log(res)
      },
      (err) => {
        console.log(err)
      })
  }



  currentDate = new Date();

  src = [
    {img_id: 1,url:"https://cf.shopee.vn/file/d4fbe0e60caeb997e1bd24126ccec5cf"},
    {img_id: 2,url:"https://dony.vn/wp-content/uploads/2021/07/ao-so-mi-nu-ban-nhieu-trn-tang-tmdt-viet-1.jpg"},
    {img_id: 3,url:"https://lzd-img-global.slatic.net/g/p/0f2d9ede901e0bcd123715e4f7daad29.jpg_720x720q80.jpg_.webp"},
    {img_id: 4,url:"https://triscy.com/wp-content/uploads/2020/02/1f510db48e651cef5d72860c7b9d43b9.jpg"}
  ]
  ImagePath = 'nothing'

  Imagehover(url:any) {
    // var filter_array = this.src.filter(x => x.img_id == n);
    // filter_array.forEach(element => {this.ImagePath = element.url
    // });
    this.ImagePath = url
  }

  QtyDecrease(n:any){
    if( n > 0 && n!=1){
      this.qty = n-1;
      this.sum_price -= this.price;
    }
    else{
      this.qty = 1;
    }
  }

  QtyIncrease(n:any){
    if(n > 0){
      this.qty = n+1;
      this.sum_price += this.price;
    }
    else{
      this.qty = 1;
    }
  }
}
