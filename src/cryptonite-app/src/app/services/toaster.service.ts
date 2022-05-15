import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ClientEvent } from '../models/client-events/client-event';
import { ToasterEvent } from '../models/client-events/toaster-event';

@Injectable({
  providedIn: 'root'
})
export class ToasterService {
  constructor(private toastr: ToastrService) {
  }

  handleToasterEvent(event: ClientEvent): void {
    const toaster: ToasterEvent = event.innerEventJson;
    let toasterType: ToasterType;
    try {
      toasterType = toaster.type as ToasterType;
    } catch (e) {
      console.error(e);
      this.toastr.error('Undefined toaster type detected', 'Unknown type');
      throw new Error(`Undefined toaster type: ${toaster.type}`);
    }

    this.show(toasterType, toaster.title, toaster.message);
  }

  show(type: ToasterType, title: string, message: string): void {
    switch (type) {
      case 'Info':
        this.toastr.info(message, title);
        break;
      case 'Warning':
        this.toastr.warning(message, title);
        break;
      case 'Error':
        this.toastr.error(message, title);
        break;
      case 'Success':
        this.toastr.success(message, title);
        break;
    }
  }
}

export type ToasterType = 'Info' | 'Warning' | 'Error' | 'Success';
