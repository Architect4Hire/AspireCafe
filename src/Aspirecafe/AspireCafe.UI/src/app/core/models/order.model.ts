export interface OrderItem {
  menuItemId: string;
  name: string;
  quantity: number;
  unitPrice: number;
  notes?: string;
}

export interface Order {
  id: string;
  items: OrderItem[];
  tableNumber?: number;
  customerName?: string;
  status: 'pending' | 'preparing' | 'ready' | 'completed' | 'cancelled';
  createdAt: Date;
  updatedAt: Date;
  total: number;
  paymentStatus: 'unpaid' | 'paid';
  paymentMethod?: 'cash' | 'card' | 'mobile';
  staffId: string;
}