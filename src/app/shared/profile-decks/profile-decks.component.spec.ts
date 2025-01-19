import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileDecksComponent } from './profile-decks.component';

describe('ProfileDecksComponent', () => {
  let component: ProfileDecksComponent;
  let fixture: ComponentFixture<ProfileDecksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProfileDecksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfileDecksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
