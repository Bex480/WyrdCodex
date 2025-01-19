import {Card} from './card.model';

export interface Deck{
	id: number;
	deckName: string,
	imageUrl: string,
	userDecks: Deck[],
	deckCards: Card[],
	cards: Card[]
}
