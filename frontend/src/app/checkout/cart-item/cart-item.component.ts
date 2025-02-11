import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Card} from '../../models/card.model';
import {CheckoutService} from '../../services/checkout.service';
import {debounceTime, distinct, distinctUntilChanged} from 'rxjs/operators';
import {FormControl} from '@angular/forms';

@Component({
  selector: 'app-cart-item',
  standalone: false,

  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.css'
})
export class CartItemComponent implements OnInit{
	@Input() card!: Card;
	@Input() price: number = 0.00;
	@Input() quantity: number = 1;
	quantityControl: FormControl;
	@Output() updatedCartItem: EventEmitter<void> = new EventEmitter<void>()
	@Input() saved: boolean = false;
	total = 0.00;

	constructor(private checkoutService: CheckoutService) {
		this.quantityControl = new FormControl(this.quantity);
	}

	ngOnInit() {
		this.quantityControl.setValue(this.quantity);
		this.total = this.price * this.quantity;

		this.quantityControl.valueChanges
			.pipe(
				debounceTime(300),
				distinctUntilChanged()
				)
			.subscribe((newQuantity) => {
				this.checkoutService.setCardQuantityInCart(this.card.id, newQuantity).subscribe();
			});
	}

	increaseQuantity() {
		let currentQuantity = this.quantityControl.value;
		this.quantityControl.setValue(currentQuantity + 1);
		this.total = this.price * this.quantityControl.value;
	}

	decreaseQuantity() {
		if (this.quantity > 1) {
			let currentQuantity = this.quantityControl.value;
			this.quantityControl.setValue(currentQuantity - 1);
			this.total = this.price * this.quantityControl.value;
		}
	}

	removeFromCart() {
		this.checkoutService.removeFromCart(this.card.id)
			.subscribe( r=> this.updatedCartItem.emit());
	}

	saveForLater() {
		this.checkoutService.addToSaveForLater(this.card.id)
			.subscribe( r=> this.updatedCartItem.emit());
	}

	sendToCart() {
		this.checkoutService.addToCart(this.card.id)
			.subscribe(_ => this.checkoutService.setCardQuantityInCart(this.card.id, this.quantity)
				.subscribe(_ => this.checkoutService.removeFromSaveForLater(this.card.id)
					.subscribe(_ => this.updatedCartItem.emit())));
	}
}
