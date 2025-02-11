import { Component } from '@angular/core';
import {Card} from '../../models/card.model';
import {CardService} from '../../services/card.service';
import {Router} from '@angular/router';


@Component({
  selector: 'app-card-details',
  standalone: false,

  templateUrl: './card-details.component.html',
  styleUrl: './card-details.component.css'
})
export class CardDetailsComponent {
	card!: Card;
	cardID:string | null = localStorage.getItem('cardID');
	cardItself: Card | undefined;

	constructor(private cardService: CardService, private router: Router) {}

	ngOnInit() {
		if (this.cardID != null) {
			this.cardService.getCardById(parseInt(this.cardID)).subscribe(
				(response: Card) => {
					console.log(response);
					this.cardItself = response;
				},
				(error) => {
					console.error('Error fetching cards:', error);
				}
			);
		}
	}

	goToCart(){
		//this.router.navigate(['/account/two-factor'], { queryParams: { email: formData.email } });
		//NAPISI OVDJE STA HOCES. IDK WHERE YOU WERE PLANNING TO PUT THE PATH AND WHAT YOU WANT TO SEND(I FORGOT), SO JUST CHANGE IT I BELIEVE YOU CAN DO IT!!!
		//IF YOU WANT TO SEND EVERY ATTRIBUTE OF THE CARD, YOU CAN JUST SEND "cardItself".
	}

}
