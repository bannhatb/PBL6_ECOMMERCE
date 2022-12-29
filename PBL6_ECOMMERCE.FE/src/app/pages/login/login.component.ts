import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BusinessService } from 'src/app/_services/business.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  form: FormGroup
  constructor(
    private fb: FormBuilder,
    private businessService: BusinessService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.makeForm()
  }

  get username(){
    return this.form.get('username')
  }

  get password(){
    return this.form.get('password')
  }

  makeForm(){
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  submitData(){
    if(this.form.valid){
      const data = this.form.value
      const submitData = {
        "username": data.username,
        "password": data.password
      }
      this.businessService.login(submitData)
      .subscribe(
        (res:any) => {
          const token = res.result.token
          this.businessService.setToken(token)
          //return home
          this.router.navigate(['/'])
        },
        (err) => {
          alert("Sai ten dang nhap hoac mat khau")
        }
      )
    }
  }
}
