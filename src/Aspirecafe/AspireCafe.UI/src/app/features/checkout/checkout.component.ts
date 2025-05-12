import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { OrderService } from '../../core/services/order.service';
import { OrderItem } from '../../core/models/order.model';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  private orderService = inject(OrderService);
  private router = inject(Router);

  currentOrderItems: OrderItem[] = [];
  orderTotal = 0;
  customerName = '';
  tableNumber?: number;
  isProcessing = false;
  showOrderSuccess = false;
  orderNumber = '';
  
  ngOnInit(): void {
    this.loadCurrentOrder();
  }

  loadCurrentOrder(): void {
    this.orderService.currentOrder$.subscribe(items => {
      this.currentOrderItems = items;
      this.calculateTotal();
    });
  }

  calculateTotal(): void {
    this.orderTotal = this.orderService.calculateTotal();
  }

  updateQuantity(menuItemId: string, quantity: number): void {
    this.orderService.updateItemQuantity(menuItemId, quantity);
  }

  removeItem(menuItemId: string): void {
    this.orderService.removeItemFromOrder(menuItemId);
  }

  clearOrder(): void {
    this.orderService.clearOrder();
    this.customerName = '';
    this.tableNumber = undefined;
  }

  placeOrder(): void {
    if (this.currentOrderItems.length === 0) {
      return;
    }

    this.isProcessing = true;
    
    this.orderService.placeOrder(this.tableNumber, this.customerName).subscribe({
      next: (order) => {
        this.isProcessing = false;
        this.showOrderSuccess = true;
        this.orderNumber = order.id.slice(-4);
        
        // Reset form after success
        this.customerName = '';
        this.tableNumber = undefined;
      },
      error: (error) => {
        console.error('Error placing order', error);
        this.isProcessing = false;
      }
    });
  }

  goToOrders(): void {
    this.showOrderSuccess = false;
    this.router.navigate(['/orders']);
  }

  continueOrdering(): void {
    this.showOrderSuccess = false;
    this.router.navigate(['/menu']);
  }
}