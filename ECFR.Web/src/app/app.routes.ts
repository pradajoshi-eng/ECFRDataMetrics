
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { App } from './app';
import { AgencyList } from './agency-list/agency-list';
import { AgencyMetrics } from './agency-metrics/agency-metrics';


export const routes: Routes = [
  { path: '', component: AgencyList  },
  { path: 'agency-metrics/:agencyId/:name', component: AgencyMetrics },
];

