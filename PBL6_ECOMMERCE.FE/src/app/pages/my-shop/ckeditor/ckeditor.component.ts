import { Component, Input, OnInit } from '@angular/core';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { Console } from 'console';


@Component({
  selector: 'app-ckeditor',
  templateUrl: './ckeditor.component.html',
  styleUrls: ['./ckeditor.component.css']
})
export class CkeditorComponent implements OnInit {
  @Input() view: 'grid'|'list' = 'grid';
  constructor() { }
  public Editor = ClassicEditor;
  public results: any; // Change it private to public
  public mymessage: any; 
  public newEditor: any;

  public onReady( editor: any ) {
    console.log(editor);
    this.Editor = editor;
    
    // editor.ui.getEditableElement().parentElement.insertBefore(
    //     editor.ui.view.toolbar.element,
    //     editor.ui.getEditableElement()
    // );
}
  
  ngOnInit(): void {
  }
  public getdata(){
    console.log(this.Editor.getData());
    
  }
  

}