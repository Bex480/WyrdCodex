import { Component } from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {CardService} from '../../services/card.service';
import {Card} from '../../models/card.model';
import {ApiConfig} from '../../config/api.config';
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
	card!: Card;
	responseGotten: boolean = false;
	isClicked: boolean = false;
	allCards: any = [];
	cardsSearched: any = [];
	cardCounter: any = 10;
	cardNumber: any = 0;
	divStates = Array(10).fill(false);
	cardIsClicked: boolean = false;

	toggleBorder2(index: number): void {
		this.divStates[index] = !this.divStates[index];
		this.cardIsClicked = !this.cardIsClicked;
	}

	constructor(private fb: FormBuilder, private http: HttpClient, private cardService: CardService, private router: Router, private sharedDataService: SharedDataService) {
		this.shopSearchForm = this.fb.group({
			card_name: [''],
			type: [''],
			faction: ['']
		})
	}

	ngOnInit(): void {
		/*this.cardService.getCards().subscribe(
			(response: Card[]) => {
				this.allCards = response;
				this.cardsSearched = response;
				this.responseGotten = true;
				this.cardNumber = this.allCards.length;
			},
			(error) => {
				console.error('Error fetching cards:', error);
			}
		) */

		console.log("hello!");

		for(let i = 6, j = 0; i < 17; i++, j++){
			this.cardService.getCardById(i).subscribe(
				(response: Card) => {
					console.log(response);
					this.allCards[j] = response;
					this.cardsSearched[j] = response;
					//this.responseGotten = true;
					this.cardNumber = this.allCards.length;
				},
				(error) => {
					console.error('Error fetching cards:', error);
				}
			);
		}
		this.responseGotten = true;

	}

	onSubmit(){
		this.getFilteredCards();
	}

	showDetails(position:number){
		localStorage.setItem("cardID", position.toString());
	}

	getFilteredCards(){
		/* this.cardsSearched = [];
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
		console.log(this.cardsSearched, this.cardNumber); */
	}

	changeNumber() {
		const newNumber = Math.floor(Math.random() * 100);
		this.sharedDataService.updateNumber(newNumber);
	}

	cycleForward(){
		if(this.cardCounter == 20)
			return;
		this.cardCounter += 10;
		console.log(this.cardCounter, this.cardNumber);
	}

	cycleBackward(){
		if(this.cardCounter == 0)
			return;
		this.cardCounter -= 10;
		console.log(this.cardCounter, this.cardNumber);
	}

	toggleBorder(){
		this.isClicked = !this.isClicked;
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
