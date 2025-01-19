import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Card} from '../../models/card.model';
import {CardService} from '../../services/card.service';
import {ApiConfig} from '../../config/api.config';
import { Deck } from '../../models/deck.model';
import {getXHRResponse} from 'rxjs/internal/ajax/getXHRResponse';


@Component({
	selector: 'app-collection',
	standalone: false,

	templateUrl: './collection.component.html',
	styleUrl: './collection.component.css'
})
export class CollectionComponent {
	collectionSearchForm: FormGroup;
	card!: Card;
	deck: Deck[] = [];
	deck2!: Deck;
	deckCards: Card[] = [];
	clickSource: string = "Collection";
	cardsSearched: any = [];

	constructor(private fb: FormBuilder, private http: HttpClient, private cardService: CardService) {
		this.collectionSearchForm = this.fb.group({
			card_name: [''],
			type: [''],
			faction: [''],
			deckSelected: ['']
		})
	}

	ngOnInit() {
		const authToken = localStorage.getItem('authToken');
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${authToken}`
		});

		this.http.get<Deck[]>(`${ApiConfig.apiUrl}/Card/decks`, { headers }).subscribe((response: Deck[]) => {
			this.deck = response;
		}, error => {
			console.error('Error:', error);
		});

		this.getAllCards();
	}


	onSubmit(){
		if(this.getDeckSelected() == ""){
			this.getAllCards();
			return;
		}

		for(let i = 0; i < this.deck.length; i++){
			if(this.deck[i].deckName == this.getDeckSelected())
				this.deck2 = this.deck[i];
		}

		this.http.get<Deck>(`${ApiConfig.apiUrl}/Card/deck?deckID=${this.deck2.id}`).subscribe((response: Deck) => {
				this.deckCards = response.cards;
				this.getFilteredCards();
			},
			error => {
				console.error('Error:', error);
			})

	}

	getFilteredCards(){
		this.cardsSearched = [];
		for(let i = 0, j = 0; i < this.deckCards.length; i++){
			if(this.deckCards[i].cardName.includes(this.getCardName())
				&& this.deckCards[i].type.includes(this.getCardType())
				&& this.deckCards[i].faction.includes(this.getCardFaction())) {
				this.cardsSearched[j] = this.deckCards[i];
				j++;
			}
		}

		this.deckCards = this.cardsSearched;
	}


	getAllCards(){
		if(this.getDeckSelected() == ""){
			this.cardService.getCards().subscribe((data: Card[]) => {
				this.deckCards = data;
				this.getFilteredCards();
			});
			return;
		}
		this.getFilteredCards();
	}

	deckChosen(deck:Deck){
		console.log(deck);
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

	getDeckSelected(){
		return this.collectionSearchForm.controls['deckSelected'].value;
	}
}
