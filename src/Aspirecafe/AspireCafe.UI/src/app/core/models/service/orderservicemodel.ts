export interface OrderServiceModel {
  header: OrderHeaderServiceModel;
  lines: OrderLineItemServiceModel[];
  footer: OrderFooterServiceModel;
}

export interface OrderLineItemServiceModel {
  productId: string;
  quantity: number;
  price: number;
  notes: string | null;
}

export interface OrderHeaderServiceModel {
  orderType: string;
  tableNumber: number | null;
  customerName: string | null;
}

export interface OrderFooterServiceModel {
  subTotal: number;
  tax: number;
  total: number;
  paymentMethod: string;
  paymentStatus: string;
  orderStatus: string;
  notes: string | null;
}
