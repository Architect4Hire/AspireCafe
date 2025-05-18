import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { delay, map } from 'rxjs/operators';
import { MenuItem } from '../models/menu-item.model';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  // Mock menu items for demo
  private menuItems: MenuItem[] = [
    {
      id: '1',
      name: 'Cappuccino',
      description: 'Espresso with steamed milk and foam',
      price: 4.5,
      category: 'coffee',
      imageUrl: 'images/cappuccino.jpg',
      available: true
    },
    {
      id: '2',
      name: 'Latte',
      description: 'Espresso with steamed milk',
      price: 4.0,
      category: 'coffee',
      imageUrl: 'images/latte.jpg',
      available: true
    },
    {
      id: '3',
      name: 'Croissant',
      description: 'Buttery, flaky pastry',
      price: 3.5,
      category: 'pastry',
      imageUrl: 'images/croissant.jpg',
      available: true
    },
    {
      id: '4',
      name: 'Blueberry Muffin',
      description: 'Moist muffin with fresh blueberries',
      price: 3.0,
      category: 'pastry',
      imageUrl: 'images/muffin.jpg',
      available: true
    },
    {
      id: '5',
      name: 'Iced Tea',
      description: 'Refreshing cold tea',
      price: 3.0,
      category: 'cold-drinks',
      imageUrl: 'images/iced-tea.jpg',
      available: true
    }
  ];

  private menuItemsSubject = new BehaviorSubject<MenuItem[]>(this.menuItems);

  getMenuItems(): Observable<MenuItem[]> {
    return this.menuItemsSubject.asObservable().pipe(delay(300)); // Simulate API delay
  }

  getMenuItem(id: string): Observable<MenuItem | undefined> {
    return this.getMenuItems().pipe(
      map(items => items.find(item => item.id === id))
    );
  }

  addMenuItem(item: Omit<MenuItem, 'id'>): Observable<MenuItem> {
    const newItem: MenuItem = {
      ...item,
      id: Date.now().toString()
    };
    this.menuItems = [...this.menuItems, newItem];
    this.menuItemsSubject.next(this.menuItems);
    return of(newItem).pipe(delay(300));
  }

  updateMenuItem(id: string, updates: Partial<MenuItem>): Observable<MenuItem> {
    const itemIndex = this.menuItems.findIndex(item => item.id === id);
    if (itemIndex === -1) {
      throw new Error('Item not found');
    }
    
    const updatedItem = { ...this.menuItems[itemIndex], ...updates };
    this.menuItems = [
      ...this.menuItems.slice(0, itemIndex),
      updatedItem,
      ...this.menuItems.slice(itemIndex + 1)
    ];
    this.menuItemsSubject.next(this.menuItems);
    return of(updatedItem).pipe(delay(300));
  }

  deleteMenuItem(id: string): Observable<void> {
    this.menuItems = this.menuItems.filter(item => item.id !== id);
    this.menuItemsSubject.next(this.menuItems);
    return of(void 0).pipe(delay(300));
  }

  getCategories(): Observable<string[]> {
    return this.getMenuItems().pipe(
      map(items => [...new Set(items.map(item => item.category))])
    );
  }
}
