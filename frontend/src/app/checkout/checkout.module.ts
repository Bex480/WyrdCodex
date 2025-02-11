import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import { CartComponent } from './cart/cart.component';
import { CartItemComponent } from './cart-item/cart-item.component';
import {CheckoutRoutingModule} from './checkout-routing.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {SharedModule} from '../shared/shared.module';
import { PaymentComponent } from './payment/payment.component';
import { SavedForLaterComponent } from './saved-for-later/saved-for-later.component';



@NgModule({
  declarations: [
    CartComponent,
    CartItemComponent,
    PaymentComponent,
    SavedForLaterComponent
  ],
	imports: [
		CommonModule,
		CheckoutRoutingModule,
		FormsModule,
		NgOptimizedImage,
		SharedModule,
		ReactiveFormsModule
	]
})
export class CheckoutModule { }
