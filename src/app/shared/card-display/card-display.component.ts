import { Component } from '@angular/core';
import { Card } from '../../models/card.model';
import { CardService } from '../../services/card.service';

@Component({
  selector: 'app-card-display',
  standalone: false,

  templateUrl: './card-display.component.html',
  styleUrl: './card-display.component.css'
})
export class CardDisplayComponent {
	card!: Card;

	constructor(private cardService: CardService) {}

	ngOnInit(): void {
		this.cardService.getCardById(5).subscribe(
			(response: Card) => {
				this.card = response;
			},
			(error) => {
				console.error('Error fetching cards:', error);
			}
		);
	}
}
