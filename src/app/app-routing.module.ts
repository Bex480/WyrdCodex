import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
	{
		path: 'account',
		loadChildren: ()=> import('./account/account.module').then(m=>m.AccountModule)
	},
	{
		path: 'dashboard',
		loadChildren: ()=>import('./dashboard/dashboard.module').then(m=>m.DashboardModule)
	},
	{
		path: 'admin',
		loadChildren: ()=>import('./admin/admin.module').then(m=>m.AdminModule)
	},
	{
		path: 'checkout',
		loadChildren: ()=>import('./checkout/checkout.module').then(m=>m.CheckoutModule)
	},
	{
		path: '**',
		loadChildren: () => import('./dashboard/dashboard.module').then(m=>m.DashboardModule)
	}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
