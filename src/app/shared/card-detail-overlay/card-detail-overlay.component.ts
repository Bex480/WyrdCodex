import {Component, Input} from '@angular/core';
import {Card} from '../../models/card.model';

@Component({
  selector: 'app-card-detail-overlay',
  standalone: false,

  templateUrl: './card-detail-overlay.component.html',
  styleUrl: './card-detail-overlay.component.css'
})
export class CardDetailOverlayComponent {
	@Input() card!: Card;

	closeOverlay(): void {
		this.card = null!;
	}
}
