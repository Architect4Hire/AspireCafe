import { OrderStatus, OrderProcessStation, OrderProcessStatus } from "../enums/enumerations";

export interface OrderUpdateServiceModel {
  orderStatus: OrderStatus;
  orderId: string;
  station: OrderProcessStation;
  cookingStatus: OrderProcessStatus;
}
