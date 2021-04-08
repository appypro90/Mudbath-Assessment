export interface Product {
    id: number;
    name: string;
    description: string;
    price: Price;
    relatedProducts: number[];
}

export interface Price {
    base: string;
    amount: number;
}