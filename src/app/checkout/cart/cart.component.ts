import {Component, Input, OnInit} from '@angular/core';
import {Card} from '../../models/card.model';
import {CheckoutService} from '../../services/checkout.service';


@Component({
  selector: 'app-cart',
  standalone: false,

  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit{
	items: { card: Card, quantity: number }[] = [];
	loading = false;
	@Input() atCheckout: boolean = true;

	constructor(private checkoutService: CheckoutService) {}

	ngOnInit() {
		if (this.atCheckout) { this.loadCart() }
		else { this.loadSavedForLater() }
	}

	loadCart() {
		this.loading = true;
		this.checkoutService.getCart().subscribe((data: { card: Card, quantity: number }[]) => {
			this.items = data;
			this.loading = false;
		});
	}

	loadSavedForLater() {
		this.loading = true;
		this.checkoutService.getSavedForLater().subscribe((data: { card: Card, quantity: number }[]) => {
			this.items = data;
			this.loading = false;
		});
	}

	cartItemUpdated() {
		if (this.atCheckout) { this.loadCart() }
		else { this.loadSavedForLater() }
	}
}
