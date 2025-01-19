import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CardService } from '../../services/card.service';
import { Card } from '../../models/card.model';
import { FormBuilder, FormGroup } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';

@Component({
	selector: 'app-card-grid',
	standalone: false,

	templateUrl: './card-grid.component.html',
	styleUrls: ['./card-grid.component.css']
})
export class CardGridComponent implements OnInit {
	cards: Card[] = [];
	loading: boolean = true;
	selectedCard?: Card;
	filterForm: FormGroup;

	@Output() cardUpdate = new EventEmitter<Card>();

	constructor(private cardService: CardService, private fb: FormBuilder) {
		this.filterForm = this.fb.group({
			cardName: [''],
			cardType: [''],
			cardFaction: ['']
		});
	}

	ngOnInit(): void {
		this.loadCards();

		this.filterForm.valueChanges
			.pipe(debounceTime(300))
			.subscribe((filters) => {
				this.loadCards(filters);
			});
	}

	loadCards(filters: any = {}): void {
		this.loading = true;
		this.cardService.getCards(filters).subscribe((data: Card[]) => {
			this.cards = data;
			this.loading = false;
		});
	}

	onCardDoubleClick(card: Card): void {
		this.selectedCard = card;
	}

	onCardUpdate(card: Card): void {
		this.cardUpdate.emit(card);
	}

	Reload(): void {
		this.loadCards(this.filterForm.value);
	}
}
