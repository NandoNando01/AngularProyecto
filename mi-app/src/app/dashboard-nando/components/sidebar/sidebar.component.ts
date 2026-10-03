import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-sidebar',
  imports: [MatIconModule],
  styleUrl: './sidebar.component.css',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly menuItems = [
    { label: 'Dashboard', icon: 'dashboard', active: true },
    { label: 'Data Security', icon: 'security', active: false },
    { label: 'Shadow IT', icon: 'visibility', active: false },
    { label: 'User Behaviour', icon: 'person', active: false },
  ];

  readonly manageItems = [
    { label: 'Destination', icon: 'near_me', active: false },
    { label: 'Protection', icon: 'verified_user', active: false },
    { label: 'Data Classification', icon: 'category', active: false },
    { label: 'Device', icon: 'devices', active: false },
  ];

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}