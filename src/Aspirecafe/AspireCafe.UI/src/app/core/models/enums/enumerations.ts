export enum OrderStatus {
  Pending = 1,
  Preparing = 2,
  Ready = 3,
  Delivered = 4,
  Cancel = 5
}

export enum OrderProcessStatus {
  Waiting = 1,
  Received = 2,
  Preparing = 3,
  Ready = 4,
  Delivered = 5,
  Cancelled = 6
}
export enum OrderProcessStation {
  Expo = 0,
  Bar = 1,
  Saute = 2,
  Grill = 3,
  Fry = 4,
  Pantry = 5,
  Vegetable = 6,
  Fish = 7,
  Rotisseur = 8
}

export enum PaymentStatus {
  Paid = 1,
  Unpaid = 2,
  Refunded = 3
}

export enum PaymentMethod {
  Cash = 1,
  Card = 2,
  MobilePayment = 3
}

export enum OrderType {
  DineIn = 1,
  TakeAway = 2,
  Delivery = 3
}
