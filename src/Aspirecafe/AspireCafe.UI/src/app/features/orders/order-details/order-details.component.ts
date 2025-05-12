import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Order } from '../../../core/models/order.model';
import { OrderService } from '../../../core/services/order.service';
import { NavbarComponent } from '../../../shared/components/navbar/navbar.component';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [CommonModule, RouterModule, NavbarComponent],
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private orderService = inject(OrderService);

  order: Order | null = null;
  isLoading = true;
  errorMessage = '';
  
  ngOnInit(): void {
    const orderId = this.route.snapshot.paramMap.get('id');
    if (!orderId) {
      this.errorMessage = 'Invalid order ID';
      this.isLoading = false;
      return;
    }
    
    this.loadOrder(orderId);
  }

  loadOrder(id: string): void {
    this.orderService.getOrder(id).subscribe({
      next: (order) => {
        if (!order) {
          this.errorMessage = 'Order not found';
          this.isLoading = false;
          return;
        }
        
        this.order = order;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading order', error);
        this.errorMessage = 'Failed to load order details';
        this.isLoading = false;
      }
    });
  }

  updateStatus(status: Order['status']): void {
    if (!this.order) return;
    
    this.orderService.updateOrderStatus(this.order.id, status).subscribe({
      next: (updatedOrder) => {
        this.order = updatedOrder;
      },
      error: (error) => {
        console.error('Error updating order status', error);
      }
    });
  }

  processPayment(method: Order['paymentMethod']): void {
    if (!this.order) return;
    
    this.orderService.completePayment(this.order.id, method).subscribe({
      next: (updatedOrder) => {
        this.order = updatedOrder;
      },
      error: (error) => {
        console.error('Error processing payment', error);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/orders']);
  }

  getStatusClass(status: Order['status']): string {
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