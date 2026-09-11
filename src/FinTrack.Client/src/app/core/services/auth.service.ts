import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginRequest } from '../models/auth/login-request';
import { Observable } from 'rxjs';
import { AuthenticationResponse } from '../models/auth/authentication-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl=`${environment.apiUrl}/authentication`;
  constructor(private http:HttpClient) { }

  login(request:LoginRequest):Observable<AuthenticationResponse>
  {
    return this.http.post<AuthenticationResponse>(
      `${this.baseUrl}/login`,request
    );
  }
}
