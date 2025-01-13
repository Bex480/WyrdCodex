import { Component } from '@angular/core';
import {Card} from '../../models/card.model';

@Component({
  selector: 'app-panel',
  standalone: false,

  templateUrl: './panel.component.html',
  styleUrl: './panel.component.css'
})
export class PanelComponent {
	currentView = "browse-cards"
	selectedCardForUpdate!: Card;

	onCardUpdate(card: Card) {
		this.currentView = "update-card"
		this.selectedCardForUpdate = card
	}

	onBrowseCards() {
		this.currentView = "browse-cards"
	}

	onAddCard() {
		this.currentView = "add-card"
	}

	onUpdateComplete() {
		this.currentView = "browse-cards"
	}
}
