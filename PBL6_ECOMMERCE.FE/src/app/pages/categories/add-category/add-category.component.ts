import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CategoryService } from 'src/app/_services/category.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnInit {

  form: FormGroup
  constructor(
    private formBuilder: FormBuilder,
    private categoriesService: CategoryService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.makeForm()
  }

  makeForm(){
    this.form = this.formBuilder.group({
      //dinh nghia cac input cua form
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]]
    })
  }
  get name() {return this.form.get('name')}

  submitData(){
    if(this.form.valid){
      const data = this.form.value
      //create category
      this.categoriesService.createCategory(data)
      .subscribe(
        (res) => {
          alert('Add category success')
          this.router.navigate(['/'])
        },
        (err) => {
          alert('Add category fail. Detail: ' + JSON.stringify(err))
        }
      )
    }
  }
}
