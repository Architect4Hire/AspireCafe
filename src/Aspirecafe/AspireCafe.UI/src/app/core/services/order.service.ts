import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { delay, map } from 'rxjs/operators';
import { Order, OrderItem } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private orders: Order[] = [];
  private ordersSubject = new BehaviorSubject<Order[]>([]);
  private currentOrderSubject = new BehaviorSubject<OrderItem[]>([]);
  
  currentOrder$ = this.currentOrderSubject.asObservable();
  orders$ = this.ordersSubject.asObservable();

  addItemToOrder(item: OrderItem): void {
    const currentOrder = this.currentOrderSubject.value;
    const existingItemIndex = currentOrder.findIndex(i => i.menuItemId === item.menuItemId);
    
    if (existingItemIndex !== -1) {
      // Update existing item quantity
      const updatedOrder = [...currentOrder];
      updatedOrder[existingItemIndex] = {
        ...updatedOrder[existingItemIndex],
        quantity: updatedOrder[existingItemIndex].quantity + item.quantity
      };
      this.currentOrderSubject.next(updatedOrder);
    } else {
      // Add new item
      this.currentOrderSubject.next([...currentOrder, item]);
    }
  }

  removeItemFromOrder(menuItemId: string): void {
    const currentOrder = this.currentOrderSubject.value;
    this.currentOrderSubject.next(
      currentOrder.filter(item => item.menuItemId !== menuItemId)
    );
  }

  updateItemQuantity(menuItemId: string, quantity: number): void {
    if (quantity <= 0) {
      this.removeItemFromOrder(menuItemId);
      return;
    }

    const currentOrder = this.currentOrderSubject.value;
    const itemIndex = currentOrder.findIndex(item => item.menuItemId === menuItemId);
    
    if (itemIndex !== -1) {
      const updatedOrder = [...currentOrder];
      updatedOrder[itemIndex] = { ...updatedOrder[itemIndex], quantity };
      this.currentOrderSubject.next(updatedOrder);
    }
  }

  clearOrder(): void {
    this.currentOrderSubject.next([]);
  }

  calculateTotal(): number {
    return this.currentOrderSubject.value.reduce(
      (total, item) => total + (item.quantity * item.unitPrice), 
      0
    );
  }

  placeOrder(tableNumber?: number, customerName?: string): Observable<Order> {
    const items = this.currentOrderSubject.value;
    
    if (items.length === 0) {
      throw new Error('Cannot place an empty order');
    }

    const newOrder: Order = {
      id: Date.now().toString(),
      items,
      tableNumber,
      customerName,
      status: 'pending',
      createdAt: new Date(),
      updatedAt: new Date(),
      total: this.calculateTotal(),
      paymentStatus: 'unpaid',
      staffId: '1' // In a real app, this would be the current user's ID
    };

    this.orders = [...this.orders, newOrder];
    this.ordersSubject.next(this.orders);
    this.clearOrder(); // Reset current order after placing it
    
    return of(newOrder).pipe(delay(500)); // Simulate API delay
  }

  getOrders(): Observable<Order[]> {
    return this.ordersSubject.asObservable().pipe(delay(300));
  }

  getOrder(id: string): Observable<Order | undefined> {
    return this.getOrders().pipe(
      map(orders => orders.find(order => order.id === id))
    );
  }

  updateOrderStatus(id: string, status: Order['status']): Observable<Order> {
    const orderIndex = this.orders.findIndex(order => order.id === id);
    if (orderIndex === -1) {
      throw new Error('Order not found');
    }
    
    const updatedOrder = { 
      ...this.orders[orderIndex], 
      status, 
      updatedAt: new Date() 
    };
    
    this.orders = [
      ...this.orders.slice(0, orderIndex),
      updatedOrder,
      ...this.orders.slice(orderIndex + 1)
    ];
    
    this.ordersSubject.next(this.orders);
    return of(updatedOrder).pipe(delay(300));
  }

  completePayment(id: string, paymentMethod: Order['paymentMethod']): Observable<Order> {
    const orderIndex = this.orders.findIndex(order => order.id === id);
    if (orderIndex === -1) {
      throw new Error('Order not found');
    }
    
    const updatedOrder = { 
      ...this.orders[orderIndex], 
      paymentStatus: 'paid' as 'paid',
      paymentMethod,
      updatedAt: new Date(),
      status: 'completed' as 'completed'
    };
    
    this.orders = [
      ...this.orders.slice(0, orderIndex),
      updatedOrder,
      ...this.orders.slice(orderIndex + 1)
    ];
    
    this.ordersSubject.next(this.orders);
    return of(updatedOrder).pipe(delay(300));
  }
}