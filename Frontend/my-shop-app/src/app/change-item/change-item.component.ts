import { Component, Inject, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Product } from '../models/product.model';
import { ExchageRate } from '../models/exchangeRates.model';
import { ExchangeRateService } from '../services/exchangeRate.service';
import { ProductsService } from '../services/products.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

@Component({
  selector: 'app-change-item',
  templateUrl: './change-item.component.html',
  styleUrls: ['./change-item.component.scss']
})
export class ChangeItemComponent implements OnInit {

  public productForm: FormGroup;
  public exchangeRates: ExchageRate[];
  public products: Product[];
  filteredProducts: Product[];
  selectedProduct: Product;
  relatedProductsControl = new FormControl();
  relatedproducts: Product[];
  separatorKeysCodes: number[] = [ENTER, COMMA];

  constructor(private _formBuilder: FormBuilder,
    private _productService: ProductsService,
    exchageRateService: ExchangeRateService,
    public dialogRef: MatDialogRef<ChangeItemComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {product: Product, isReadonly: boolean}) {

    this.selectedProduct = data.product;
    this.products = _productService.getproducts();
    this.filteredProducts = _productService.getproducts();
    this.exchangeRates = exchageRateService.getExchageRates();
    this.relatedproducts = this.products.filter(p => data.product.relatedProducts?.indexOf(p.id) >= 0)

    this.productForm = this._formBuilder.group({
      id: [data.product?.id, {
        validators: [Validators.required, this.uniqueIdValidator]
      }],
      name: [data.product?.name, {
        validators: [Validators.required, Validators.minLength(3)],
      }],
      description: [data.product?.description],
      priceBase: [data.product?.price.base, { validators: [Validators.required] }],
      priceAmount: [data.product?.price.amount, { validators: [Validators.required] }],
      relatedproducts: [data.product?.relatedProducts],
    });

    if(data.isReadonly){
      this.productForm.disable();
      this.relatedProductsControl.disable();
    }
  }

  remove = (product: Product): void => {
    const index = this.relatedproducts?.indexOf(product);

    if (index >= 0) {
      this.relatedproducts?.splice(index, 1);
    }
    this.populateRelatedPorductsControl();
  }

  selected = (event: MatAutocompleteSelectedEvent): void => {
    const selectedProduct = this.products.filter(p => p.name.toLocaleLowerCase() === event.option.value.toLocaleLowerCase());
    this.relatedproducts.push(selectedProduct[0]);
    this.populateRelatedPorductsControl();
    this.relatedProductsControl.setValue(null);
  }

  populateRelatedPorductsControl = () => {
    const relatedProductIds: number[] = [];
    this.relatedproducts?.forEach(prod => relatedProductIds.push(prod.id));
    this.productForm.controls.relatedproducts.setValue(relatedProductIds)
  }

  ngOnInit(): void {
    this.relatedProductsControl.valueChanges.subscribe(e => this.filteredProducts = this.products.filter(x =>
      x.name.toLocaleLowerCase().indexOf(e?.toLocaleLowerCase()) >= 0));
  }

  submitProduct = () => {
    this.selectedProduct.id = this.productForm.value.id;
    this.selectedProduct.name = this.productForm.value.name;
    this.selectedProduct.description = this.productForm.value.description;
    this.selectedProduct.relatedProducts = this.productForm.value.relatedproducts;
    this.selectedProduct.price.base = this.productForm.value.priceBase;
    this.selectedProduct.price.amount = +this.productForm.value.priceAmount;
    this._productService.updateproduct(this.selectedProduct);
    this.dialogRef.close();
  }

  uniqueIdValidator = (control: AbstractControl): { [key: string]: any } | null => {
    const forbidden = this.products.filter(prod => prod.id === +control.value && prod.id !== this.selectedProduct.id).length > 0;
    return forbidden ? { forbiddenName: { value: control.value } } : null;
  }
}
