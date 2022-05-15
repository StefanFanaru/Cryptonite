import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ShouldLoginComponent } from './components/should-login/should-login.component';
import { FormBuilder, FormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSliderModule } from '@angular/material/slider';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { ToastrModule } from 'ngx-toastr';
import { ErrorHandlerModule } from './core/errors/error-handler.module';
import { SpinnerOverlayService } from './core/shared/loading/spinner-overlay.service';
import { DialogService } from './services/dialog.service';
import { SearchService } from './services/search.service';
import { ClientEventsService } from './services/client-events.service';
import { ThemingService } from './services/theming.service';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MatDialogModule } from '@angular/material/dialog';
import { SpinnerOverlayComponent } from './core/shared/loading/spinner-overlay/spinner-overlay.component';
import { SharedModule } from './core/shared/shared.module';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { LOADING_BAR_CONFIG, LoadingBarModule } from '@ngx-loading-bar/core';
import { HeaderComponent } from './components/header/header.component';
import { TabsMenuComponent } from './components/tabsmenu/tabs-menu.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { AuthModule } from './core/auth/auth.module';
import { AuthService } from './core/auth/auth.service';
import { CryptoDataService } from './services/crypto-data.service';
import { ChartsModule } from 'ng2-charts';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HeaderComponent,
    TabsMenuComponent,
    SpinnerOverlayComponent,
    ShouldLoginComponent
  ],
  entryComponents: [],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    IonicModule.forRoot(),
    AppRoutingModule,
    MatTooltipModule,
    MatButtonModule,
    MatRippleModule,
    MatIconModule,
    MatSliderModule,
    MatDialogModule,
    MatSlideToggleModule,
    FormsModule,
    ErrorHandlerModule,
    PerfectScrollbarModule,
    LoadingBarHttpClientModule,
    LoadingBarModule,
    ChartsModule,
    AuthModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 4000,
      preventDuplicates: true,
      countDuplicates: true,
      resetTimeoutOnDuplicate: true,
      maxOpened: 3,
      progressBar: true,
      closeButton: false,
      extendedTimeOut: 1000,
      progressAnimation: 'decreasing',
      positionClass: 'toast-position'
    }),
    ServiceWorkerModule.register('cryptonite-worker.js', {
      enabled: true,
      // Register the ServiceWorker as soon as the app is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:10000'
    })
  ],
  providers: [
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
    {
      provide: LOADING_BAR_CONFIG,
      useValue: {
        latencyThreshold: 300
      }
    },
    AuthService,
    SpinnerOverlayService,
    DialogService,
    SearchService,
    ClientEventsService,
    ThemingService,
    FormBuilder,
    CryptoDataService
  ],
  bootstrap: [AppComponent],
  exports: [
    SharedModule,
    MatSlideToggleModule,
    MatDialogModule,
    MatIconModule,
    MatSliderModule,
    MatTooltipModule,
    MatButtonModule,
    MatRippleModule,
    MatIconModule,
    FormsModule,
    PerfectScrollbarModule
  ]
})
export class AppModule {
}
