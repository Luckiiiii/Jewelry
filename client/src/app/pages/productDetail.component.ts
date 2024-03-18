import { Component, OnInit } from "@angular/core";
import { Store } from "../services/store.service";
import { Product } from "../shared/Product";
import { ActivatedRoute } from "@angular/router";


@Component({
    selector: "product-details",
    templateUrl: "productDetail.component.html",
})
export default class ProductDetail implements OnInit {
    product: Product;

    constructor(private route: ActivatedRoute, public store: Store) { }

    ngOnInit(): void {
        //const productId = +this.route.snapshot.paramMap.get("id");
        //this.product = this.store.products.find(p => p.id === productId);
    }
}