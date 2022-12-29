import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MyShopRoutingModule } from './my-shop-routing.module';
import { MyShopHeaderComponent } from './my-shop-header/my-shop-header.component';
import { AddProductComponent } from './add-product/add-product.component';
import { CkeditorComponent } from './ckeditor/ckeditor.component';
import { ViewListProductComponent } from './view-list-product/view-list-product.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ManageOrderComponent } from './manage-order/manage-order.component';
import { RevenueComponent } from './revenue/revenue.component';
import {NgxPaginationModule} from 'ngx-pagination';


@NgModule({
  declarations: [
    MyShopHeaderComponent,
    AddProductComponent,
    CkeditorComponent,
    ViewListProductComponent,
    ProductDetailComponent,
    ManageOrderComponent,
    RevenueComponent

  ],
  imports: [
    CommonModule,
    MyShopRoutingModule,
    NgxPaginationModule
  ]
})
export class MyShopModule { }
