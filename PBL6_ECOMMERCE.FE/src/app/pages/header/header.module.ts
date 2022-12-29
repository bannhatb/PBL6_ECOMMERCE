import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderRoutingModule } from './header-routing.module';

import { HomeComponent } from 'src/app/pages/home/home.component';
import { LoginComponent } from 'src/app/pages/login/login.component';
import { DetailComponent } from 'src/app/pages/detail/detail.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HeaderRoutingModule
  ],
  declarations: [


  ]
})
export class HeaderModule {}
