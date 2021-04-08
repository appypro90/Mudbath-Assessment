import exchangeRates from '../../../../my-shop-app/exchange_rates.json';
import { ExchageRate } from '../models/exchangeRates.model';

export class ExchangeRateService {
    private _exchageRates = exchangeRates;

    public getExchageRates = (): ExchageRate[] => this._exchageRates;
}