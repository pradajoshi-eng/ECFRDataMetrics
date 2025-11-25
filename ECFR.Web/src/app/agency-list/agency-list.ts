import { Component, OnInit, Inject, Injectable } from '@angular/core';
import { EcfrDataService } from '../../services/e-cfrdata-service';
import { Agency } from "../../model/agency.model";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router'; 

@Component({
  selector: 'agency-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './agency-list.html',
  styleUrl: './agency-list.css',
  standalone: true,
})
export class AgencyList implements OnInit {
  agencies: Agency[] = [];
  constructor(private svc: EcfrDataService, private router: Router) { }
  ngOnInit() {
    this.svc.getAgencies().subscribe({
      next: (response: any) => {
        this.agencies = response || [];
        console.log('Data loaded successfully:', this.agencies);
      },
      error: (err) => console.error('Error getting agencies:', err)
    });

    console.log(this.agencies);
    //(x => this.agencies = x);
  }

  select(a: Agency) {
    console.log('Navigating to:', a.name);
    this.router.navigate(['/agency-metrics'], {queryParams: { agencyId: a.agencyId, name: a.name }
    });
  }

  viewMetrics(id: number, name: string) {
    console.log('Navigating to:', id);
    this.router.navigate(['/agency-metrics', id, name]);
  }
}
