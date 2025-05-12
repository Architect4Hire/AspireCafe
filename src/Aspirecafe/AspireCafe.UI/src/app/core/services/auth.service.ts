import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { delay, tap } from 'rxjs/operators';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private router = inject(Router);
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  // Mock users for demo purposes
  private users: User[] = [
    {
      id: '1',
      username: 'admin',
      email: 'admin@cafe.com',
      role: 'admin',
      fullName: 'Admin User'
    },
    {
      id: '2',
      username: 'staff',
      email: 'staff@cafe.com',
      role: 'staff',
      fullName: 'Staff User'
    }
  ];

  constructor() {
    // Check if user is already logged in
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUserSubject.next(JSON.parse(storedUser));
    }
  }

  login(username: string, password: string): Observable<User> {
    // In a real app, this would be an API call
    const user = this.users.find(u => u.username === username);
    
    if (user && password === 'password') { // Simple password check for demo
      localStorage.setItem('currentUser', JSON.stringify(user));
      this.currentUserSubject.next(user);
      return of(user).pipe(
        delay(500), // Simulate API delay
        tap(() => this.router.navigate(['/dashboard']))
      );
    }
    
    return throwError(() => new Error('Invalid username or password'));
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  isAuthenticated(): boolean {
    return !!this.currentUser;
  }

  hasRole(role: 'admin' | 'staff'): boolean {
    return this.currentUser?.role === role;
  }
}