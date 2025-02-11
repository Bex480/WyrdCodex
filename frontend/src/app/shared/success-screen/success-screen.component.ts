import {Component, Input} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-success-screen',
  standalone: false,

  templateUrl: './success-screen.component.html',
  styleUrl: './success-screen.component.css'
})
export class SuccessScreenComponent {
	@Input() buttonText: string = 'Home';
	@Input() route: string = '/home';

	constructor(private router: Router) {}
	navigateToHome() {
		this.router.navigate([this.route]);
	}
}
