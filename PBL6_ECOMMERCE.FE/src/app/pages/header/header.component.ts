import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/app-user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  user : User  = new User();
  username :any;
  active  = false;
  token  :any ;
  myarr :any;
  accountOpen = false;
  dataSearch : string ='';
  constructor(
    public accountService: AccountService,
    private router :Router
    ) {}


  ngOnInit(): void {

    this.token = localStorage.getItem("token");
    console.log(this.token);
    if (this.token != null)
    {
      this.active = true;
    }
    else{
      this.active = false;
    }
    // this.myarr = this.token.split('"');
    // this.username = this.myarr[3];
    // console.log(this.username);
  }
  logout(){
    this.accountService.logout();
    window.location.reload();

  }
  search(){
    let data = this.dataSearch
    this.router.navigate(['/search'] ,{ queryParams: { data },  skipLocationChange: true });
    console.log(this.dataSearch);

  }
}
