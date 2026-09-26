import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { API_BASE_URL } from '../core/api.config';
import { AuthResponse, AuthUserData, LoginRequest, RegisterRequest } from '../models/auth.model';

const TOKEN_KEY = 'auth_token';
const USER_KEY = 'user_data';

function canUseStorage(): boolean {
  return typeof window !== 'undefined';
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${inject(API_BASE_URL)}/auth`;

  private readonly userSignal = signal<AuthUserData | null>(this.readUserFromStorage());

  readonly currentUser = this.userSignal.asReadonly();

  login(payload: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, payload)
      .pipe(tap((response) => this.saveSession(response.data)));
  }

  register(payload: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, payload)
      .pipe(tap((response) => this.saveSession(response.data)));
  }

  logout(): void {
    if (canUseStorage()) {
      window.localStorage.removeItem(TOKEN_KEY);
      window.localStorage.removeItem(USER_KEY);
    }
    this.userSignal.set(null);
  }

  getToken(): string | null {
    if (!canUseStorage()) {
      return null;
    }
    return window.localStorage.getItem(TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return Boolean(this.getToken());
  }

  private readUserFromStorage(): AuthUserData | null {
    if (!canUseStorage()) {
      return null;
    }
    const raw = window.localStorage.getItem(USER_KEY);
    if (!raw) {
      return null;
    }
    try {
      return JSON.parse(raw) as AuthUserData;
    } catch {
      return null;
    }
  }

  private saveSession(data?: AuthUserData): void {
    if (!data) {
      return;
    }
    if (canUseStorage()) {
      window.localStorage.setItem(TOKEN_KEY, data.token);
      window.localStorage.setItem(USER_KEY, JSON.stringify(data));
    }
    this.userSignal.set(data);
  }
}
