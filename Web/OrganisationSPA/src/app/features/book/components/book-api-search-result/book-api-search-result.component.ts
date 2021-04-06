import {Component, Input, OnInit, Output, EventEmitter} from '@angular/core';
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";
import {BookApiEditModalComponent} from "../book-api-edit-modal/book-api-edit-modal.component";
import {CreateBookUsingApiDto} from "../../models/create-book-using-api-dto";
import {first} from "rxjs/operators";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";
import {Author} from "../../models/author";

@Component({
  selector: 'app-book-api-search-result',
  templateUrl: './book-api-search-result.component.html',
  styleUrls: ['./book-api-search-result.component.sass']
})
export class BookApiSearchResultComponent implements OnInit {

  @Output() addEvent = new EventEmitter<CreateBookUsingApiDto>();
  @Input() searchResult: SearchResultDto;
  @Input() uploaderStyle: IFileUploaderStyle;
  modalRef: BsModalRef;

  constructor(private modalService: BsModalService) { }

  ngOnInit(): void {

  }

  edit(volumeInfo: any): void {
    const initialState = {
      volumeInfo: volumeInfo,
      uploaderStyle: this.uploaderStyle
    };
    this.modalRef = this.modalService.show(BookApiEditModalComponent, {initialState});
    this.modalRef.setClass('modal-lg')
    this.modalRef.content.volumeInfo$.subscribe((res:CreateBookUsingApiDto) => {
      this.addEvent.emit(res);
    })
  }

  add(volumeInfo: any): void {
    const book: CreateBookUsingApiDto = {
      title: volumeInfo?.title,
      description: volumeInfo?.description,
      isbn10: volumeInfo?.industryIdentifiers?.filter(x => x.type === 'ISBN_10')[0]?.identifier,
      isbn13: volumeInfo?.industryIdentifiers.filter(x => x.type === 'ISBN_13')[0]?.identifier,
      pageCount: volumeInfo?.pageCount,
      visibility: false,
      languageName: volumeInfo?.language,
      author: {
        firstName: volumeInfo?.authors?.toString(),
        lastName: volumeInfo?.authors?.toString(),
      },
      publisherName: volumeInfo?.publisher,
      categoriesNames: volumeInfo?.categories,
      publishedDate: new Date(volumeInfo?.publishedDate)
    }
    this.addEvent.emit(book)
    // console.log(this.searchResult.items.filter(x => x.volumeInfo.id === 'PlegDwAAQBAJ')[0].volumeInfo)
  }


}


export interface SearchResultDto {
  totalItems: number;
  items: any[];
}
