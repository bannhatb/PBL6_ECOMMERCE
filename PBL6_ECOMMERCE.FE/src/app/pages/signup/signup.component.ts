import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterUser } from 'src/app/_models/app-user';
import { AccountService } from 'src/app/_services/account.service';
@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  registerUser : RegisterUser = new RegisterUser;
  gender : any;
  constructor(
    private accountService : AccountService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }
  register(){
    this.registerUser.gender = (this.gender) ? true :false;
    this.registerUser.role =3;
    console.log(this.registerUser);
    this.accountService.register(this.registerUser).subscribe();
    this.router.navigate(['/'])
  }
}
