import { Component } from '@angular/core';
import { Product } from './models/product.model';
import { ExchageRate } from './models/exchangeRates.model';
import { MatDialog } from '@angular/material/dialog';
import { ChangeItemComponent } from './change-item/change-item.component';
import { ProductsService } from './services/products.service';
import { ExchangeRateService } from './services/exchangeRate.service';
import { CurrencyService } from './services/currency.service';
import { MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'my-shop-app';

  public items: Product[];
  public exchangeRates: ExchageRate[];
  public selectedCurrency: ExchageRate;

  constructor(private _productService: ProductsService,
    exchageRateService: ExchangeRateService,
    private _currencyService: CurrencyService,
    public dialog: MatDialog) {
    this.selectedCurrency = _currencyService.selectedCurrency;
    this.items = _productService.getproducts();
    this.exchangeRates = exchageRateService.getExchageRates();
  }

  getPrice = (item: Product): number => this._currencyService.getPrice(item);

  openDialog = (item: Product, isReadonly: boolean): void => {
    const dialogRef = this.dialog.open(ChangeItemComponent, {
      width: '30vw',
      data: {product: item, isReadonly: isReadonly}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.items = this._productService.getproducts();
    });
  }

  changesSelectedCurrency = (e: MatSelectChange) => {
    this._currencyService.setSelectedCurrency(e.value);
  }

}
