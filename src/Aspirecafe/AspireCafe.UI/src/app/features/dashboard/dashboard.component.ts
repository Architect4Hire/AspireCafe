import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { OrderService } from '../../core/services/order.service';
import { Order } from '../../core/models/order.model';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, NavbarComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private orderService = inject(OrderService);

  userName = '';
  recentOrders: Order[] = [];
  pendingOrdersCount = 0;
  todaySales = 0;
  today = new Date();
  
  ngOnInit(): void {
    this.userName = this.authService.currentUser?.fullName || '';
    
    this.orderService.getOrders().subscribe(orders => {
      // Get recent orders (last 5)
      this.recentOrders = orders
        .sort((a, b) => b.createdAt.getTime() - a.createdAt.getTime())
        .slice(0, 5);
      
      // Calculate pending orders
      this.pendingOrdersCount = orders.filter(o => 
        o.status === 'pending' || o.status === 'preparing'
      ).length;
      
      // Calculate today's sales
      this.today = new Date();
      this.today.setHours(0, 0, 0, 0);
      
      this.todaySales = orders
        .filter(o => 
          o.paymentStatus === 'paid' && 
          new Date(o.createdAt).getTime() >= this.today.getTime()
        )
        .reduce((sum, order) => sum + order.total, 0);
    });
  }

  getOrderStatusClass(status: Order['status']): string {
    switch (status) {
      case 'pending': return 'status-pending';
      case 'preparing': return 'status-preparing';
      case 'ready': return 'status-ready';
      case 'completed': return 'status-completed';
      case 'cancelled': return 'status-cancelled';
      default: return '';
    }
  }
}