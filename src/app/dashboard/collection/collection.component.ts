import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {Card} from '../../models/card.model';
import {CardService} from '../../services/card.service';

@Component({
	selector: 'app-collection',
	standalone: false,

	templateUrl: './collection.component.html',
	styleUrl: './collection.component.css'
})
export class CollectionComponent {
	collectionSearchForm: FormGroup;
	showAdvanced : boolean = false;
	card!: Card;
	responseGotten: boolean = false;
	allCards: any;
	cardsSearched: any = [];
	cardCounter: any = 10;
	cardNumber: any = 0;

	constructor(private fb: FormBuilder, private http: HttpClient, private cardService: CardService) {
		this.collectionSearchForm = this.fb.group({
			card_name: [''],
			type: [''],
			faction: ['']
		})
	}

	ngOnInit(): void {
		 this.cardService.getCards().subscribe(
			 (response: Card[]) => {
				 this.allCards = response;
				 this.cardsSearched = response;
				 this.responseGotten = true;
				 this.cardNumber = this.allCards.length;
			 },
			 (error) => {
				 console.error('Error fetching cards:', error);
			 }
		 )
	}


	onSubmit(){
		this.getFilteredCards();
	}

	getFilteredCards(){
		this.cardsSearched = [];
		for(let i = 0, j = 0; i < this.allCards.length; i++){
			if(this.allCards[i].cardName.includes(this.getCardName())
				&& this.allCards[i].type.includes(this.getCardType())
				&& this.allCards[i].faction.includes(this.getCardFaction())) {
					this.cardsSearched[j] = this.allCards[i];
					j++;
					this.cardNumber = j;
					if(j <= 10)
						this.cardCounter = 10;
			}
		}
		console.log(this.cardsSearched, this.cardNumber);
	}

	cycleForward(){
		if(this.cardCounter == 20)
			return;
		this.cardCounter += 10;
	}

	cycleBackward(){
		if(this.cardCounter == 0)
			return;
		this.cardCounter -= 10;
	}
	getCardName(){
		return this.collectionSearchForm.controls['card_name'].value;
	}

	getCardType() {
		return this.collectionSearchForm.controls['type'].value;
	}

	getCardFaction(){
		return this.collectionSearchForm.controls['faction'].value;
	}
}
