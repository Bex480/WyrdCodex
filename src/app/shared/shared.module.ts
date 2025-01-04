import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { NavigationbarComponent } from './navigationbar/navigationbar.component';
import {ReactiveFormsModule} from "@angular/forms";
import { LoadingScreenComponent } from './loading-screen/loading-screen.component';
import { SuccessScreenComponent } from './success-screen/success-screen.component';
import { ProfileOptionComponent } from './profile-components/profile-option/profile-option.component';
import { SettingsOptionComponent } from './profile-components/settings-option/settings-option.component';


@NgModule({
  declarations: [
    NavigationbarComponent,
    LoadingScreenComponent,
    SuccessScreenComponent,
    ProfileOptionComponent,
    SettingsOptionComponent
  ],
    imports: [
        CommonModule,
        SharedRoutingModule,
        ReactiveFormsModule
    ],
	exports: [
		NavigationbarComponent,
		LoadingScreenComponent,
		SuccessScreenComponent,
		ProfileOptionComponent
	]
})
export class SharedModule { }
