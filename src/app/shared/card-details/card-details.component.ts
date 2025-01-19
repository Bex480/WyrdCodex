import {Component, Input} from '@angular/core';
import {Card} from '../../models/card.model';
import {CardService} from '../../services/card.service';
import {Router} from '@angular/router';
import {Deck} from '../../models/deck.model';


@Component({
	selector: 'app-card-details',
	standalone: false,

	templateUrl: './card-details.component.html',
	styleUrl: './card-details.component.css'
})
export class CardDetailsComponent {
	@Input() card!: Card;
	@Input() deck!: Deck;
	@Input() sourceOfClick: string = "";

	constructor(private cardService: CardService, private router: Router) {}

	ngOnInit() {

	}

	closeOverlay(): void {
		this.card = null!;
		this.deck = null!;
	}
	goToCart(){
		//this.router.navigate(['/account/two-factor'], { queryParams: { email: formData.email } });
		//NAPISI OVDJE STA HOCES. IDK WHERE YOU WERE PLANNING TO PUT THE PATH AND WHAT YOU WANT TO SEND(I FORGOT), SO JUST CHANGE IT I BELIEVE YOU CAN DO IT!!!
		//IF YOU WANT TO SEND EVERY ATTRIBUTE OF THE CARD, YOU CAN JUST SEND "cardItself".
	}

}
