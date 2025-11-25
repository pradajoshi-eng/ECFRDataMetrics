import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Agency, CfrReference, Snapshot } from '../model/agency.model';

@Injectable({
  providedIn: 'root' 
})
export class EcfrDataService {
  private base = 'https://localhost:7296/api'; // proxy config to backend during dev or full URL in production

  constructor(private http: HttpClient) {}

  getAgencies(): Observable<Agency[]> { return this.http.get<Agency[]>(`${this.base}/agency/GetAgenciesData`); }

  fetchAgency(agencyId: number, displayName: string, passedDate: string, title: string, additionalQSParams?: string) {
    return this.http.post(`${this.base}/datafetcher/FetchAndStoreAgencyDataSnapshot`, { agencyId: agencyId, displayName, passedDate, title, additionalQSParams } );
  }

  getLatest(agencyId: number) {
    return this.http.get(`${this.base}/metrics/agency/${agencyId}/latest`);
  }

  getSeries(agencyId: number, from?: string, to?: string) {
    const params: any = {};
    if (from) params.from = from;
    if (to) params.to = to;
    return this.http.get<Snapshot[]>(`${this.base}/metrics/agency/${agencyId}/series`, { params });
  }

  diff(leftId: number, rightId: number) {
    return this.http.get(`${this.base}/metrics/snapshot/diff`, { params: { leftId, rightId } });
  }
}
