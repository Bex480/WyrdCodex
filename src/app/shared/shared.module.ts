import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { NavigationbarComponent } from './navigationbar/navigationbar.component';
import {ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [
    NavigationbarComponent
  ],
    imports: [
        CommonModule,
        SharedRoutingModule,
        ReactiveFormsModule
    ],
  exports:[
    NavigationbarComponent
  ]
})
export class SharedModule { }
