import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Card} from '../../models/card.model';
import {ApiConfig} from '../../config/api.config';
import {HttpClient, HttpHeaders} from '@angular/common/http';

@Component({
  selector: 'app-card-detail-overlay',
  standalone: false,

  templateUrl: './card-detail-overlay.component.html',
  styleUrl: './card-detail-overlay.component.css'
})
export class CardDetailOverlayComponent {
	@Input() card!: Card;
	@Output() updateCard: EventEmitter<Card> = new EventEmitter<Card>();
	@Output() reloadGrid: EventEmitter<void> = new EventEmitter();


	constructor(private http: HttpClient) {
	}

	onUpdateClick() {
		if (this.card) {
			this.updateCard.emit(this.card);
		}
	}

	onDeleteClick() {
		this.http.delete(`${ApiConfig.apiUrl}/Card?cardID=${this.card.id}`)
			.subscribe( (r:any) => {this.reloadGrid.emit();});
		this.closeOverlay();
	}

	closeOverlay(): void {
		this.card = null!;
	}
}
