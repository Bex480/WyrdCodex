import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient, withFetch} from '@angular/common/http';
import {AuthInterceptor} from './services/auth.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [
    provideHttpClient(
      withFetch(),
    ),
	  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
   provideAnimationsAsync()
  ],
  exports: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
