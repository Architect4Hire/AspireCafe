import { OrderType, PaymentMethod, PaymentStatus, OrderStatus } from "../enums/enumerations";
import { LineItemViewModel } from "./lineitem";

export interface OrderViewModel {
  orderId: string | null;
  orderType: OrderType;
  tableNumber: number | null;
  customerName: string | null;
  items: LineItemViewModel[];
  subTotal: number;
  tax: number;
  tip: number;
  total: number;
  paymentMethod: PaymentMethod;
  paymentStatus: PaymentStatus;
  orderStatus: OrderStatus;
  notes: string | null;
}
