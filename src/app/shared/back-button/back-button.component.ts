import {Component, Input} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-back-button',
  standalone: false,

  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.css'
})
export class BackButtonComponent {
	@Input() route = ""

	constructor(private router: Router) {}

	onBackButtonClicked(){
		this.router.navigate([this.route]);
	}
}
