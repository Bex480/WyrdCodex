import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {MainComponent} from './main/main.component';
import {CollectionComponent} from './collection/collection.component';
import {ProfileComponent} from '../account/profile/profile.component';
import {ShopComponent} from './shop/shop.component';

const routes: Routes = [
	{
		path: '',
		component: MainComponent
	},
	{
		path: 'main',
		component: MainComponent
	},
	{
		path: 'collection',
		component: CollectionComponent
	},
	{
		path: 'shop',
		component: ShopComponent
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class DashboardRoutingModule { }
