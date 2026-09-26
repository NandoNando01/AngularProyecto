import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatToolbarModule,
    MatListModule,
    MatChipsModule,
    MatIconModule,
    MatRippleModule,
  ],
  styleUrl: './dashboard.component.css',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly currentUser = this.authService.currentUser;

  readonly actividades = [
    { name: 'Inicio de sesión exitoso', icon: 'login', status: 'Completado' },
    { name: 'Verificación de perfil', icon: 'verified_user', status: 'Completado' },
    { name: 'Actualización de datos', icon: 'update', status: 'Completado' },
  ];

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}