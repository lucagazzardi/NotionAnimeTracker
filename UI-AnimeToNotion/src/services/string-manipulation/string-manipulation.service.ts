import { Injectable } from "@angular/core";

@Injectable()
export class StringManipulationService {

  constructor() { }

  normalize(title: string) {
    return title.normalize("NFC").replace(/[^a-zA-Z0-9 ]/g, '').split(' ').join('-');
  }
}

