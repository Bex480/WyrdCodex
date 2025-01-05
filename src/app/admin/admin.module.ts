import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PanelComponent } from './panel/panel.component';
import {AdminRoutingModule} from './admin-routing.module';
import {SharedModule} from '../shared/shared.module';



@NgModule({
  declarations: [
    PanelComponent
  ],
	imports: [
		CommonModule,
		AdminRoutingModule,
		SharedModule
	]
})
export class AdminModule { }
