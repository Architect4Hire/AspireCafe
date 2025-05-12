import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuService } from '../../../core/services/menu.service';
import { OrderService } from '../../../core/services/order.service';
import { MenuItem } from '../../../core/models/menu-item.model';
import { NavbarComponent } from '../../../shared/components/navbar/navbar.component';

@Component({
  selector: 'app-menu-list',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.css']
})
export class MenuListComponent implements OnInit {
  private menuService = inject(MenuService);
  private orderService = inject(OrderService);

  menuItems: MenuItem[] = [];
  filteredItems: MenuItem[] = [];
  categories: string[] = [];
  selectedCategory: string = '';
  searchTerm: string = '';
  isLoading = true;
  
  ngOnInit(): void {
    this.loadMenuItems();
    this.loadCategories();
  }

  loadMenuItems(): void {
    this.isLoading = true;
    this.menuService.getMenuItems().subscribe({
      next: (items) => {
        this.menuItems = items;
        this.applyFilters();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading menu items', error);
        this.isLoading = false;
      }
    });
  }

  loadCategories(): void {
    this.menuService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: (error) => {
        console.error('Error loading categories', error);
      }
    });
  }

  applyFilters(): void {
    let items = this.menuItems;
    
    // Filter by category
    if (this.selectedCategory) {
      items = items.filter(item => item.category === this.selectedCategory);
    }
    
    // Filter by search term
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      items = items.filter(item => 
        item.name.toLowerCase().includes(term) ||
        item.description.toLowerCase().includes(term)
      );
    }
    
    // Only show available items
    items = items.filter(item => item.available);
    
    this.filteredItems = items;
  }

  onCategoryChange(category: string): void {
    this.selectedCategory = category;
    this.applyFilters();
  }

  onSearch(): void {
    this.applyFilters();
  }

  addItemToOrder(item: MenuItem): void {
    this.orderService.addItemToOrder({
      menuItemId: item.id,
      name: item.name,
      quantity: 1,
      unitPrice: item.price
    });
  }
}