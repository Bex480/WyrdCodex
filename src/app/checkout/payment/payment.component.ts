import { Component } from '@angular/core';
import {Router} from '@angular/router';
import {CheckoutService} from '../../services/checkout.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-payment',
  standalone: false,

  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {
	checkoutForm: FormGroup;
	loading = false;
	success = false;

	constructor(
		private router: Router,
		private checkoutService: CheckoutService,
		private fb: FormBuilder
	) {
		this.checkoutForm = this.fb.group({
			fullName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
			cardNumber: ['', [Validators.required, Validators.pattern(/^\d{16}$/)]],
			expiryDate: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])\/?([0-9]{2})$/)]],
			cvv: ['', [Validators.required, Validators.pattern(/^\d{3}$/)]]
		});
	}

	goToSavedForLater() {
		this.router.navigate(['/checkout/saved']);
	}

	onCheckout() {
		if (this.checkoutForm.invalid) {
			return;
		}

		this.loading = true;
		this.checkoutService.checkout().subscribe(
			(_) => {
				this.success = true;
				this.loading = false;
			},
			(error) => {
				this.loading = false;
				console.error('Checkout failed', error);
			}
		);
	}
}
