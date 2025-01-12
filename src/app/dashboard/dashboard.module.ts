import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { MainComponent } from './main/main.component';
import {ReactiveFormsModule} from "@angular/forms";
import {SharedModule} from '../shared/shared.module';
import { CollectionComponent } from './collection/collection.component';
import { ShopComponent } from './shop/shop.component';


@NgModule({
  declarations: [
    MainComponent,
    CollectionComponent,
    ShopComponent
  ],
	imports: [
		CommonModule,
		DashboardRoutingModule,
		ReactiveFormsModule,
		SharedModule,
		NgOptimizedImage
	]
})
export class DashboardModule { }
