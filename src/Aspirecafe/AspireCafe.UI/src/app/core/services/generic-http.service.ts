import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GenericHttpService {
  constructor(private http: HttpClient) {}

  /**
   * Resolves a string token to a well-known URL from environment settings, or returns the string if not found.
   */
  private resolveUrl(tokenOrUrl: string): string {
    if (environment.apiUrls && environment.apiUrls[tokenOrUrl]) {
      return environment.apiUrls[tokenOrUrl];
    }
    return tokenOrUrl;
  }

  get<T>(tokenOrUrl: string, params?: HttpParams | {[param: string]: string | number | boolean}, headers?: HttpHeaders | {[header: string]: string | string[]}): Observable<T> {
    const url = this.resolveUrl(tokenOrUrl);
    return this.http.get<T>(url, { params, headers });
  }

  post<T>(tokenOrUrl: string, body: any, headers?: HttpHeaders | {[header: string]: string | string[]}): Observable<T> {
    const url = this.resolveUrl(tokenOrUrl);
    return this.http.post<T>(url, body, { headers });
  }

  put<T>(tokenOrUrl: string, body: any, headers?: HttpHeaders | {[header: string]: string | string[]}): Observable<T> {
    const url = this.resolveUrl(tokenOrUrl);
    return this.http.put<T>(url, body, { headers });
  }

  delete<T>(tokenOrUrl: string, headers?: HttpHeaders | {[header: string]: string | string[]}): Observable<T> {
    const url = this.resolveUrl(tokenOrUrl);
    return this.http.delete<T>(url, { headers });
  }
}
