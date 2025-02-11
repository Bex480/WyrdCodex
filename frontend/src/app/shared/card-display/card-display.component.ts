import {Component, Input} from '@angular/core';
import { Card } from '../../models/card.model';
import { CardService } from '../../services/card.service';

@Component({
  selector: 'app-card-display',
  standalone: false,

  templateUrl: './card-display.component.html',
  styleUrl: './card-display.component.css'
})
export class CardDisplayComponent {
	@Input() card!: Card;
}
