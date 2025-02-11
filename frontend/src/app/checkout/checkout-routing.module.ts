import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {CartComponent} from './cart/cart.component';
import {PaymentComponent} from './payment/payment.component';
import {SavedForLaterComponent} from './saved-for-later/saved-for-later.component';

const routes: Routes = [
	{
		path: "saved",
		component: SavedForLaterComponent
	},
	{
		path: "payment",
		component: PaymentComponent
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class CheckoutRoutingModule { }
