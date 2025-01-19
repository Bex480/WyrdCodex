import {Component, Input} from '@angular/core';
import {Card} from '../../models/card.model';
import {CardService} from '../../services/card.service';

@Component({
	selector: 'app-card-carousel',
	standalone: false,

	templateUrl: './card-carousel.component.html',
	styleUrl: './card-carousel.component.css'
})
export class CardCarouselComponent {
	cards: Card[] = [];
	loading: boolean = true;
	Details: boolean = true;
	selectedCard?: Card;

	@Input() filteredCards: Card[] = [];
	@Input() sourceOfClick: string = "";

	constructor(private cardService: CardService) {
	}

	ngOnInit(): void {
		this.cardService.getCards().subscribe((data: Card[]) => {
			this.cards = data;
			this.loading = false;
		});
	}

	ngOnChanges(){
		this.cards = this.filteredCards;
	}

	showDetails(card: Card){
		this.Details = !this.Details;
		this.selectedCard = card;
	}
}
