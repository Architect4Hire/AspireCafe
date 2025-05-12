import { Routes } from '@angular/router';

export const MENU_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./menu-list/menu-list.component').then(c => c.MenuListComponent)
  }
];