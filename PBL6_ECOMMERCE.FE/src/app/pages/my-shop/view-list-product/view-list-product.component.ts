import { ViewEncapsulation } from '@angular/compiler';
import { Component,ViewChild, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
// import {MatTableDataSource} from "@angular/material/table";
// import {MatPaginator} from "@angular/material/paginator";
// import {MatSort} from "@angular/material/sort";
@Component({
  selector: 'app-view-list-product',
  templateUrl: './view-list-product.component.html',
  styleUrls: ['./view-list-product.component.css'],


  styles: [
		`
			.dark-modal .modal-content {
				background-color: #292b2c;
				color: white;
			}
			.dark-modal .close {
				color: white;
			}
			.light-blue-backdrop {
				background-color: #5cb3fd;
			}
		`,
  ]
})
export class ViewListProductComponent implements OnInit {

  closeResult: string;
  constructor(private modalService: NgbModal) { }

  ngOnInit(): void {

  }
  show_detail(content:any){
    console.log("HULLO")
    this.modalService.open(content, { size: 'lg' });
  }

}
export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

//Hãy nhớ add interface vào trong object ELEMENT_DATA nhé

const ELEMENT_DATA: PeriodicElement[] = [
  { position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H' },
  { position: 2, name: 'Helium', weight: 4.0026, symbol: 'He' },
  { position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li' },
  { position: 4, name: 'Beryllium', weight: 9.0122, symbol: 'Be' },
  { position: 5, name: 'Boron', weight: 10.811, symbol: 'B' },
  { position: 6, name: 'Carbon', weight: 12.0107, symbol: 'C' },
  { position: 7, name: 'Nitrogen', weight: 14.0067, symbol: 'N' },
  { position: 8, name: 'Oxygen', weight: 15.9994, symbol: 'O' },
  { position: 9, name: 'Fluorine', weight: 18.9984, symbol: 'F' },
  { position: 10, name: 'Neon', weight: 20.1797, symbol: 'Ne' },
];