import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthResponse } from '../models/auth.model';
import { AuthService } from './auth.service';

const API = 'http://localhost:5169/api';

function authPayload(): AuthResponse {
  return {
    success: true,
    message: 'Inicio de sesión exitoso.',
    data: {
      id: 1,
      email: 'usuario@test.com',
      fullName: 'Carlos Rodríguez',
      token: 'jwt-token',
      expiresAt: '2027-01-01T00:00:00Z',
    },
  };
}

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('login posts to /auth/login and saves the session', () => {
    let received: AuthResponse | undefined;
    const body = { email: 'usuario@test.com', password: 'MiContraseña#1' };

    service.login(body).subscribe((response) => (received = response));

    const request = httpMock.expectOne(`${API}/auth/login`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual(body);
    request.flush(authPayload());

    expect(received?.success).toBe(true);
    expect(service.getToken()).toBe('jwt-token');
    expect(service.currentUser()?.fullName).toBe('Carlos Rodríguez');
    expect(service.isAuthenticated()).toBe(true);
  });

  it('register posts to /auth/register and saves the session', () => {
    const body = { fullName: 'Ana García', email: 'ana@example.com', password: 'Segura#123' };

    service.register(body).subscribe();

    const request = httpMock.expectOne(`${API}/auth/register`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual(body);
    request.flush({ ...authPayload(), data: { ...authPayload().data!, email: 'ana@example.com' } });

    expect(service.getToken()).toBe('jwt-token');
    expect(service.currentUser()?.email).toBe('ana@example.com');
  });

  it('logout clears the stored session', () => {
    service.login({ email: 'a@b.com', password: 'Segura#123' }).subscribe();
    httpMock.expectOne(`${API}/auth/login`).flush(authPayload());

    expect(service.isAuthenticated()).toBe(true);

    service.logout();

    expect(service.isAuthenticated()).toBe(false);
    expect(service.currentUser()).toBeNull();
  });
});
