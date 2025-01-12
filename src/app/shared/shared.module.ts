import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { NavigationbarComponent } from './navigationbar/navigationbar.component';
import {ReactiveFormsModule} from "@angular/forms";
import { LoadingScreenComponent } from './loading-screen/loading-screen.component';
import { SuccessScreenComponent } from './success-screen/success-screen.component';
import { CardDisplayComponent } from './card-display/card-display.component';
import { AddCardFormComponent } from './add-card-form/add-card-form.component';


@NgModule({
  declarations: [
    NavigationbarComponent,
    LoadingScreenComponent,
    SuccessScreenComponent,
    CardDisplayComponent,
    AddCardFormComponent
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
		AddCardFormComponent
	]
})
export class SharedModule { }
