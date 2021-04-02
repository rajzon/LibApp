import {Component, Input, OnInit, TemplateRef} from '@angular/core';
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";
import {BookApiEditModalComponent} from "../book-api-edit-modal/book-api-edit-modal.component";
import {AbstractControl} from "@angular/forms";
import { isRequiredField } from '@shared/helpers/forms/is-required-field.function';

@Component({
  selector: 'app-book-api-search-result',
  templateUrl: './book-api-search-result.component.html',
  styleUrls: ['./book-api-search-result.component.sass']
})
export class BookApiSearchResultComponent implements OnInit {

  @Input() searchResult: SearchResult;
  modalRef: BsModalRef;

  constructor(private modalService: BsModalService) { }

  ngOnInit(): void {
  }

  edit(volumeInfo: any): void {
    const initialState = {volumeInfo: volumeInfo};
    this.modalRef = this.modalService.show(BookApiEditModalComponent, {initialState});
  }


}


export interface SearchResult {
  totalItems: number;
  items: any[];
}
