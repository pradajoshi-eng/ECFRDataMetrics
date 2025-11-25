
import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EcfrDataService } from '../../services/e-cfrdata-service';
import { Agency, Snapshot } from "../../model/agency.model";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'agency-metrics',
  imports: [CommonModule],
  templateUrl: './agency-metrics.html',
  styleUrl: './agency-metrics.css',
  standalone: true,
})
export class AgencyMetrics implements OnInit {

  series: any[] | null = null;
  protected agencyName:any = '';
  constructor(private svc: EcfrDataService, private route: ActivatedRoute) { }

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('agencyId'));
    this.agencyName = this.route.snapshot.paramMap.get('name');
    console.log('id: ', id);
    if (id) {
      this.loadSeries(id);
    }
  }

  loadSeries(agencyId: number) {
    console.log('loading metrics for:', agencyId);
    this.svc.getSeries(agencyId).subscribe({
      next: (response: any) => {
        this.series = response || [];
        console.log('Metrics loaded successfully:', this.series);
      },
      error: (err) => console.error('Error getting metrics:', err)
    });
  }
}
