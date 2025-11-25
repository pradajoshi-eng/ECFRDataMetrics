import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AgencyList } from './agency-list/agency-list';
import { AgencyMetrics } from './agency-metrics/agency-metrics';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, AgencyList, AgencyMetrics],
  templateUrl: './app.html',
  styleUrl: './app.css',
  standalone: true
})
export class App {
  protected readonly title = signal('Code of Federal Regulations - Metrics');
}
