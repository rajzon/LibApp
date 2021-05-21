import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError} from "rxjs/operators";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request)
      .pipe(catchError(error => {
        if (error instanceof HttpErrorResponse) {

          const serverError = error.error;
          if(serverError.errors && typeof serverError.errors === 'object') {
            console.log(serverError.errors);
            return throwError(serverError.errors);
          }


          return throwError(serverError || 'Server Error')
        }

      }))
  }
}
