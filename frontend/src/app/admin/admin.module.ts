import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PanelComponent } from './panel/panel.component';
import {AdminRoutingModule} from './admin-routing.module';
import {SharedModule} from '../shared/shared.module';
import {AddCardFormComponent} from './add-card-form/add-card-form.component';
import { UpdateCardFormComponent } from './update-card-form/update-card-form.component';
import {ReactiveFormsModule} from '@angular/forms';



@NgModule({
	declarations: [
		PanelComponent,
		AddCardFormComponent,
  UpdateCardFormComponent
	],
	imports: [
		CommonModule,
		AdminRoutingModule,
		SharedModule,
		ReactiveFormsModule
	]
})
export class AdminModule { }
