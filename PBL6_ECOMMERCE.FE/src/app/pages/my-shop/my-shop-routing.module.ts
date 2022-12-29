import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { DefaultComponent } from 'src/app/layouts/default/default.component';
import { AddProductComponent } from './add-product/add-product.component';
import { CkeditorComponent } from './ckeditor/ckeditor.component';
import { ViewListProductComponent } from './view-list-product/view-list-product.component';
const routes: Routes = [
  // {
  //   path: '',
  //   pathMatch: 'full',
  //   redirectTo: "addproduct"
  // },
  {
    path: '',
    component: DefaultComponent,
    children:[
      {
        path: 'addproduct',
        component: AddProductComponent,
      },
      {
        path: 'ckeditor',
        component: CkeditorComponent
      },
      {
        path: 'products',
        component: ViewListProductComponent
      },
      
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes), CKEditorModule],
  exports: [RouterModule, CKEditorModule]
})
export class MyShopRoutingModule { 


}
