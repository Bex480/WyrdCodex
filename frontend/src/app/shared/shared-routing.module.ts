import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SuccessScreenComponent} from './success-screen/success-screen.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SharedRoutingModule { }
