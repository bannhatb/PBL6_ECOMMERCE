import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-myaccount',
  templateUrl: './myaccount.component.html',
  styleUrls: ['./myaccount.component.css']
})
export class MyaccountComponent implements OnInit {

  select = 'myaccount';
  myaccountSelect = "info";
  url: string = "/info";
  urlSafe: SafeResourceUrl | undefined;
  constructor(public sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
  }
  selected(element : string){
    this.select = element;
    if(element == "myaccount"){
      this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
    }
    else if(element == "myoder"){
      this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl("/myorder");
    }
    else{
      this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
    }
  }

  myaccountElement(element : string){
    this.myaccountSelect = element;
    if(this.myaccountSelect == 'info'){
      this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl("/info");
    }
    else if(this.myaccountSelect== 'address'){
      // this.url = "/myaddress";
      this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl("/myaddress");
      console.log(this.url);
    }
    else {
      // this.url = "/changepassword";
      this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl("/changepassword");
    }
  }


}


