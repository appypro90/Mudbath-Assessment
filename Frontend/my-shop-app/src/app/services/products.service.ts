import products from '../../../../my-shop-app/products.json';
import { Product } from '../models/product.model';


export class ProductsService {
    private _products: Product[] = products;

    public getproducts = (): Product[] => {
        const sessionData = sessionStorage.getItem('products');
        if (!!sessionData) {
            this._products = JSON.parse(sessionData)
        }

        return this._products;
    }

    public updateproduct = (product: Product): void => {
        const previousProduct = this._products.filter(prod => prod.id === product.id)[0];
        const index = this._products.indexOf(previousProduct);
        if (index !== -1) {
            this._products[index] = product;
        }
        sessionStorage.setItem('products', JSON.stringify(this._products))
    }

}