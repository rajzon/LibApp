// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.


export const environment = {
  production: false,
  logoImg: 'assets/img/Logo-placeholder.png',
  bookApiUrl: 'https://localhost:5001/',
  book: {
    title: {
      required: true,
      minLength: 3,
      maxLength: 50,
    },
    authorFirstName: {
      required: true,
      minLength: 3,
      maxLength: 30,
    },
    authorLastName: {
      required: true,
      minLength: 3,
      maxLength: 30,
    },
    pageCount: {
      required: false,
      min: 10,
      max: 65535
    },
    languageName: {
      required: false,
      minLength: 2,
      maxLength: 20,
      pattern: '[a-zA-Z]*'
    },
    isbn10 : {
      required: false,
      minLength: 10,
      maxLength: 10,
      pattern: '^[0-9]+$'
    },
    isbn13 : {
      required: false,
      minLength: 13,
      maxLength: 13,
      pattern: '^[0-9]+$'
    },
    publisherName: {
      required: false,
      minLength: 2,
      maxLength: 40,
    },
    publishedDate: {
      required: false
    },
    publicSiteLink: {
      required: false,
      pattern: '(https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|www\\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9]+\\.[^\\s]{2,}|www\\.[a-zA-Z0-9]+\\.[^\\s]{2,})'
    },
    description: {
      required: false,
      minValue: 3,
      maxValue: 10000
    }



  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
