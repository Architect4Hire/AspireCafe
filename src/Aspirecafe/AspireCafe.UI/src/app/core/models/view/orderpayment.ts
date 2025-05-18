import { PaymentMethod } from "../enums/enumerations";

export interface OrderPaymentViewModel {
  orderId: string;
  paymentMethod: PaymentMethod;
  subTotal: number;
  checkAmount: number;
  tipAmount: number;
}
