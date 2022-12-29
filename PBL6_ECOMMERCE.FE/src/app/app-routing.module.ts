import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { CartComponent } from './pages/cart/cart.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { DefaultComponent } from './layouts/default/default.component';
import { AddCategoryComponent } from './pages/categories/add-category/add-category.component';
import { EditCategoryComponent } from './pages/categories/edit-category/edit-category.component';
import { MyaccountComponent } from './pages/myaccount/myaccount.component';
import { InfoComponent } from './pages/myaccount/info/info.component';
import { ChangepasswordComponent } from './pages/myaccount/changepassword/changepassword.component';
import { MyaddressComponent } from './pages/myaccount/myaddress/myaddress.component';
import { AuthGuard } from './_guards/auth.guard';
import { SignupComponent } from './pages/signup/signup.component';
import { OrderComponent } from './pages/order/order.component';
import { MyorderComponent } from './pages/myaccount/myorder/myorder.component';
import { DetailComponent } from './pages/detail/detail.component';
import { ProductDetailComponent } from './pages/home/products/product-detail/product-detail.component';
import { ShopViewComponent } from './pages/shop-view/shop-view.component';
import { SearchComponent } from './pages/search/search.component';
import { HeaderComponent } from './pages/header/header.component';
// import { MyShopHeaderComponent } from './pages/my-shop/my-shop-header/my-shop-header.component';
// import { AddProductComponent } from './pages/my-shop/add-product/add-product.component';
// import { ViewListProductComponent } from './pages/my-shop/view-list-product/view-list-product.component';
// import { ManageOrderComponent } from './pages/my-shop/manage-order/manage-order.component';
import { MyShopHeaderComponent } from './pages/my-shop/my-shop-header/my-shop-header.component';
import { AddProductComponent } from './pages/my-shop/add-product/add-product.component';
import { ViewListProductComponent } from './pages/my-shop/view-list-product/view-list-product.component';
import { ManageOrderComponent } from './pages/my-shop/manage-order/manage-order.component';
import { RevenueComponent } from './pages/my-shop/revenue/revenue.component';

const routes: Routes = [
  // {
  //   path: '',
  //   component: DefaultComponent,
  //   children: [
  //     {
  //       path: '',
  //       component: HomeComponent
  //     },
  //     {
  //       path: 'cart',
  //       component: CartComponent
  //     },
  //     {
  //       path: 'my-shop',
  //       component: MyshopComponent
  //     },
  //     {
  //       path: 'login',
  //       component: LoginComponent
  //     },
  //     {
  //       path: 'add-product',
  //       component: AddProductComponent
  //     },
  //     {
  //       path: 'ckeditor',
  //       component: CkeditorComponent
  //     },
  //     {
  //       path: 'add-category',
  //       component: AddCategoryComponent,
  //       canActivate: [AuthGuard]
  //     },
  //     {
  //       path: 'edit-category/:id',
  //       component: EditCategoryComponent
  //     },
  //     {
  //       path: 'myaccount',
  //       component: MyaccountComponent
  //     },
  //     {
  //       path: 'info',
  //       component: InfoComponent
  //     },
  //     {
  //       path: 'changepassword',
  //       component: ChangepasswordComponent
  //     },
  //     {
  //       path: 'myaddress',
  //       component: MyaddressComponent
  //     },
  //     {
  //       path: 'signup',
  //       component: SignupComponent
  //     },
  //     {
  //       path: 'order',
  //       component: OrderComponent
  //     },
  //     {
  //       path: 'myorder',
  //       component: MyorderComponent
  //     },
  //     {
  //       path: 'detail',
  //       component: DetailComponent
  //     },
  //     {
  //       path: 'product-detail/:id',
  //       component: ProductDetailComponent
  //     },
  //     {
  //       path: 'shop-view',
  //       component: ShopViewComponent
  //     },
  //     {
  //       path: 'search',
  //       component: SearchComponent
  //     },
  //   ]
  // },
  {
    path : '',
    loadChildren: () =>
        import('./pages/header/header.module').then(
          (m) => m.HeaderModule
        ),
      data: { preload: true },
  },
  {
    path:'login',
    component : LoginComponent
  },
  {
    path:'signup',
    component : SignupComponent
  },
  {
    path: 'info',
    component: InfoComponent
  },
  {
    path: 'changepassword',
    component: ChangepasswordComponent
  },
  {
    path: 'myaddress',
    component: MyaddressComponent
  },
  {
    path: 'myshop',
    component: MyShopHeaderComponent,
    children: [
      {
        path: 'addproduct',
        component: AddProductComponent
      },
      {
        path: 'products',
        component: ViewListProductComponent
      },
      {
        path: "manageorder",
        component: ManageOrderComponent
      },
      {
        path: "revenue",
        component: RevenueComponent
      }
    ]
  }

  // {
  //   path: '',
  //   component: DefaultComponent,
  //   children: [
  //     {
  //       path: '',
  //       component: HomeComponent
  //     },
  //     {
  //       path: 'cart',
  //       component: CartComponent
  //     },
  //     {
  //       path: 'login',
  //       component: LoginComponent
  //     },
  //     {
  //       path: 'add-category',
  //       component: AddCategoryComponent,
  //       canActivate: [AuthGuard]
  //     },
  //     {
  //       path: 'edit-category/:id',
  //       component: EditCategoryComponent
  //     },
  //     {
  //       path: 'myaccount',
  //       component: MyaccountComponent
  //     },
  //     {
  //       path: 'info',
  //       component: InfoComponent
  //     },
  //     {
  //       path: 'changepassword',
  //       component: ChangepasswordComponent
  //     },
  //     {
  //       path: 'myaddress',
  //       component: MyaddressComponent
  //     },
  //     {
  //       path: 'signup',
  //       component: SignupComponent
  //     },
  //     {
  //       path: 'order',
  //       component: OrderComponent
  //     },
  //     {
  //       path: 'myorder',
  //       component: MyorderComponent
  //     },
  //     {
  //       path: 'detail',
  //       component: DetailComponent
  //     },
  //     {
  //       path: 'product-detail/:id',
  //       component: ProductDetailComponent
  //     },
  //     {
  //       path: 'shop-view',
  //       component: ShopViewComponent
  //     },
  //   ]
  // }
];

@NgModule({
  imports: [RouterModule.forRoot(routes),
    CKEditorModule],
  exports: [RouterModule, CKEditorModule]
})
export class AppRoutingModule { }
