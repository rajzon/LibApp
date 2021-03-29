export const environment = {
  production: true,
  logoImg: 'assets/img/Logo-placeholder.png',
  bookApiUrl: 'https://localhost:5001/',
  book: {
    title: {
      required: true,
      minLength: 3,
      maxLength: 50,
    },
    author: {
      required: true,
      authorFirstName: {
        required: true,
        minLength: 3,
        maxLength: 30,
      },
      authorLastName: {
        required: true,
        minLength: 3,
        maxLength: 30,
      }
    },
    categories: {
      required: false,
      name: {
        required: false,
        minLength: 3,
        maxLength: 30,
      }
    },
    pageCount: {
      required: false,
      min: 10,
      max: 65535
    },
    language: {
      required: false,
      languageName: {
        required: false,
        minLength: 2,
        maxLength: 20,
        pattern: '[a-zA-Z]*'
      },
    },
    isbn10: {
      required: false,
      minLength: 10,
      maxLength: 10,
      pattern: '^[0-9]+$'
    },
    isbn13: {
      required: false,
      minLength: 13,
      maxLength: 13,
      pattern: '^[0-9]+$'
    },
    publisher: {
      required: false,
      publisherName: {
        required: false,
        minLength: 2,
        maxLength: 40,
      }
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
      minLength: 3,
      maxLength: 10000
    }
  }
};
