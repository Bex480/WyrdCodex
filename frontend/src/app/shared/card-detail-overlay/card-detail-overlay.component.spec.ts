import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardDetailOverlayComponent } from './card-detail-overlay.component';

describe('CardDetailOverlayComponent', () => {
  let component: CardDetailOverlayComponent;
  let fixture: ComponentFixture<CardDetailOverlayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CardDetailOverlayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardDetailOverlayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
