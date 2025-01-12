import {Component, OnInit} from '@angular/core';
import {CardService} from '../../services/card.service';
import {Card} from '../../models/card.model';

@Component({
  selector: 'app-card-grid',
  standalone: false,

  templateUrl: './card-grid.component.html',
  styleUrl: './card-grid.component.css'
})
export class CardGridComponent implements OnInit {
	cards: Card[] = [];
	loading: boolean = true;
	selectedCard?: Card;

	constructor(private cardService: CardService) {}

	ngOnInit(): void {
		this.cardService.getCards().subscribe((data: Card[]) => {
			this.cards = data;
			this.loading = false;
		});
	}

	onCardDoubleClick(card: Card): void {
		this.selectedCard = card;
	}

}
