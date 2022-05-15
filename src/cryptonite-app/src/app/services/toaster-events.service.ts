import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ClientEvent } from '../models/client-events/client-event';
import { ToasterEvent } from '../models/client-events/toaster-event';

@Injectable({
  providedIn: 'root'
})
export class ToasterEventsService {
  constructor(private toastr: ToastrService) {
  }

  handleToasterEvent(event: ClientEvent) {
    let toaster: ToasterEvent = event.innerEventJson;
    let toasterType: ToasterType;
    try {
      toasterType = toaster.type as ToasterType;
    } catch (e) {
      console.error(e);
      this.toastr.error('Undefined toaster type detected', 'Unknown type');
      throw new Error(`Undefined toaster type: ${toaster.type}`);
    }

    this.showToaster(toasterType, toaster.title, toaster.message);
  }

  showToaster(type: ToasterType, title: string, message: string) {
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
