import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { finalize, share } from 'rxjs/operators';
import { defer, NEVER } from 'rxjs';
import { SpinnerOverlayComponent } from './spinner-overlay/spinner-overlay.component';

// Source: https://gist.github.com/roseyda
@Injectable({
  providedIn: 'root'
})
export class SpinnerOverlayService {
  public readonly spinner$ = defer(() => {
    this.show();
    return NEVER.pipe(
      finalize(() => {
        this.hide();
      })
    );
  }).pipe(share());
  private overlayRef: OverlayRef;

  constructor(private overlay: Overlay) {
  }

  public show(): void {
    // Hack avoiding `ExpressionChangedAfterItHasBeenCheckedError` error
    Promise.resolve(null).then(() => {
      this.overlayRef = this.overlay.create({
        positionStrategy: this.overlay.position().global().centerHorizontally().centerVertically(),
        hasBackdrop: false
      });
      this.overlayRef.attach(new ComponentPortal(SpinnerOverlayComponent));
    });
  }

  public hide(): void {
    this.overlayRef?.detach();
  }
}
