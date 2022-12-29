import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from 'src/app/pages/login/login.component';
import { HomeComponent } from 'src/app/pages/home/home.component';
import { SignupComponent } from 'src/app/pages/signup/signup.component';
import { DetailComponent } from 'src/app/pages/detail/detail.component';
import { FooterComponent } from '../footer/footer.component';
import { HeaderComponent } from './header.component';
import { ProductDetailComponent } from '../home/products/product-detail/product-detail.component';
import { CartComponent } from '../cart/cart.component';
import { OrderComponent } from '../order/order.component';
import { MyaccountComponent } from '../myaccount/myaccount.component';
import { SearchComponent } from '../search/search.component';
import { AuthGuard } from 'src/app/_guards/auth.guard';
import { ShopViewComponent } from '../shop-view/shop-view.component';


const headerRoutes: Routes = [
  {
    path : '',
    component : HeaderComponent ,
    children: [
        {
          path: '',
          component: HomeComponent,
        },
        {
          path: 'product-detail/:id',
          component: ProductDetailComponent,
        },
        {
          path: 'cart',
          component: CartComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'order',
          component: OrderComponent,
          canActivate: [
            AuthGuard
            ]
        },
        {
          path :'myaccount',
          component : MyaccountComponent,
          canActivate: [
            AuthGuard
            ]
        },
        {
          path :'search',
          component : SearchComponent,
        },
        {
          path :'shop-view',
          component : ShopViewComponent,
        }
        // {
        //   path: 'login',
        //   component: LoginComponent,
        //   children: [
        //     {
        //       path: 'signup',
        //       component:SignupComponent ,
        //       }
        //     ,
        //     {
        //       path: 'detail/:id',
        //       component: DetailComponent
        //     }
        //   ]
        // }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(headerRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class HeaderRoutingModule { }

