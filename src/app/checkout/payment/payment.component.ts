import { Component } from '@angular/core';
import {Router} from '@angular/router';
import {CheckoutService} from '../../services/checkout.service';

@Component({
  selector: 'app-payment',
  standalone: false,

  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {
	loading = false;
	success = false

	constructor(private router: Router, private checkoutService: CheckoutService) {}

	goToSavedForLater() {
		this.router.navigate(["/checkout/saved"])
	}

	onCheckout() {
		this.loading = true;
		this.checkoutService.checkout().subscribe(
			_ => {
				this.success = true;
				this.loading = false;
			}
		);
	}
}
