import { Component, OnInit } from '@angular/core';

import { AuthService } from '../../core/auth/auth.service';
import { Router } from '@angular/router';
import { filter, switchMap, tap } from 'rxjs/operators';

@Component({
  selector: 'app-should-login',
  templateUrl: './should-login.component.html',
  styleUrls: ['./should-login.component.scss']
})
export class ShouldLoginComponent implements OnInit {
  constructor(private router: Router, private authService: AuthService) {
  }

  public login() {
    this.router.navigate(['/']);
  }

  ngOnInit(): void {
    this.authService.isDoneLoading$
      .pipe(
        filter(isDone => isDone),
        switchMap(_ => this.authService.isAuthenticated$),
        tap(isAuthenticated => {
          if (isAuthenticated) {
            this.router.navigate(['/']);
          }
        })
      )
      .subscribe();
  }
}
