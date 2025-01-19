import {Component} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {CardService} from '../../services/card.service';
import {Card} from '../../models/card.model';
import {Router} from '@angular/router';
import {SharedDataService} from '../../services/shared-data.service';

@Component({
	selector: 'app-shop',
	standalone: false,

	templateUrl: './shop.component.html',
	styleUrl: './shop.component.css'
})
export class ShopComponent {
	shopSearchForm: FormGroup;
	showAdvanced : boolean = false;
	cards: Card[] = [];
	loading: boolean = true;
	selectedCard?: Card;
	cardsSearched: any = [];
	cardNumber: any = 0;
	clickSource: string = "Shop";

	constructor(private fb: FormBuilder, private http: HttpClient, private cardService: CardService, private router: Router, private sharedDataService: SharedDataService) {
		this.shopSearchForm = this.fb.group({
			card_name: [''],
			type: [''],
			faction: ['']
		})
	}

	ngOnInit(): void {
		this.cardService.getCards().subscribe((data: Card[]) => {
			this.cards = data;
			this.loading = false;
		});
	}



	onSubmit(){
		this.getFilteredCards();
	}

	getFilteredCards(){
		this.cardsSearched = [];
		for(let i = 0, j = 0; i < this.cards.length; i++){
			if(this.cards[i].cardName.includes(this.getCardName())
				&& this.cards[i].type.includes(this.getCardType())
				&& this.cards[i].faction.includes(this.getCardFaction())) {
				this.cardsSearched[j] = this.cards[i];
				j++;
			}
		}

		console.log(this.cardsSearched, this.cardNumber);
	}

	onCardDoubleClick(card: Card): void {
		this.selectedCard = card;
	}

	getCardName(){
		return this.shopSearchForm.controls['card_name'].value;
	}

	getCardType() {
		return this.shopSearchForm.controls['type'].value;
	}

	getCardFaction(){
		return this.shopSearchForm.controls['faction'].value;
	}
}
