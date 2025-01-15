import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { NavigationbarComponent } from './navigationbar/navigationbar.component';
import {ReactiveFormsModule} from "@angular/forms";
import { LoadingScreenComponent } from './loading-screen/loading-screen.component';
import { SuccessScreenComponent } from './success-screen/success-screen.component';
import { CardDisplayComponent } from './card-display/card-display.component';
import {CardDetailsComponent} from './card-details/card-details.component';
import { CardGridComponent } from './card-grid/card-grid.component';
import { CardDetailOverlayComponent } from './card-detail-overlay/card-detail-overlay.component';
import { BackButtonComponent } from './back-button/back-button.component';


@NgModule({
	declarations: [
		NavigationbarComponent,
		LoadingScreenComponent,
		SuccessScreenComponent,
		CardDisplayComponent,
		CardDetailsComponent,
  CardGridComponent,
  CardDetailOverlayComponent,
  BackButtonComponent
	],
    imports: [
        CommonModule,
        SharedRoutingModule,
        ReactiveFormsModule,
        NgOptimizedImage
    ],
	exports: [
		NavigationbarComponent,
		LoadingScreenComponent,
		SuccessScreenComponent,
		CardDisplayComponent,
		CardDetailsComponent,
		CardGridComponent,
		BackButtonComponent
	]
})
export class SharedModule { }
