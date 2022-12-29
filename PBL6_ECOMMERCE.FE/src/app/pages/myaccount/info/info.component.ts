import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/app-user';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.css']
})
export class InfoComponent implements OnInit {

  avtUrl ='./assets/img/userAvatar/Avatar.jpg';
  userEmail : any;
  userPhone:any;
  userAvatar: any;
  userDateOFBirth :any;
  userGender:any;
  userFirstName :any;
  userLastName:any;
  ngay : Date = new Date()
  info :
    {
      "email": string,
      "phone": number,
      "urlAvatar": string,
      "dateOfBirth": Date,
      "gender": Boolean,
      "firstName": string,
      "lastName": string
    }

  constructor(
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.loadInfo()
    console.log(this.info);
    console.log(this.ngay);


  }
  loadInfo(){
    this.userService.getInfoUser()
    .subscribe(
      (res) => this.handleGetInfoUserSuccess(res),
      (err) => this.handleGetInfoUserError(err)
    )

  }


  handleGetInfoUserError(err: any){
    console.log(err)
  }
  handleGetInfoUserSuccess(res: any){
    this.info = res.result.data
    console.log(this.info)
    this.userEmail = this.info.email;
    this.userPhone = this.info.phone;
    this.userAvatar = this.info.urlAvatar;
    this.userDateOFBirth = this.info.dateOfBirth;
    this.userGender = this.info.gender;
    this.userFirstName = this.info.firstName;
    this.userLastName = this.info.lastName;
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
