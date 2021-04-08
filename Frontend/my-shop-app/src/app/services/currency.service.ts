import { Injectable } from "@angular/core";
import { DictionaryHelper } from "../helpers/dictionary.helper";
import { ExchageRate } from "../models/exchangeRates.model";
import { Product } from "../models/product.model";
import { ExchangeRateService } from './exchangeRate.service';

@Injectable()
export class CurrencyService {
    public selectedCurrency: ExchageRate;
    exchangeRates: ExchageRate[];

    constructor(private _dictionaryHelper: DictionaryHelper, private _exchangerateService: ExchangeRateService) {
        this.selectedCurrency = _exchangerateService.getExchageRates()[0];
        this.exchangeRates = _exchangerateService.getExchageRates();
    }

    public getPrice = (item: Product): number => {
        if (item.price.base !== this.selectedCurrency.base) {
            const rates = this._dictionaryHelper.convertObjectToDictionary(this.exchangeRates.find(x => x.base === this.selectedCurrency.base)?.rates);
            return item.price.amount * rates.find(r => r.name === item.price.base)?.value;
        }
        return item.price.amount;
    }

    public setSelectedCurrency = (currency: string) => {
        this.selectedCurrency = this.exchangeRates.filter(e => e.base === currency)[0];
    }
}