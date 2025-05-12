import { Routes } from '@angular/router';

export const ORDERS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./orders-list/orders-list.component').then(c => c.OrdersListComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./order-details/order-details.component').then(c => c.OrderDetailsComponent)
  }
];