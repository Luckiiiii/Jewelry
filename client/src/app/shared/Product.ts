export class ProductItem {
    id: number;
    product: Product;
    size: Size;
    quantity: number;
    material: Material;
    purchasePrice: PurchasePrice[];
    salesPrice: SalesPrice[];
}

export class ProductCategory {
    id: number;
    name: string;
}

export class Size {
    id: number;
    name: string;
}

export class Material {
    id: number;
    name: string;
}

export class PurchasePrice {
    id: number;
    productItem: ProductItem;
    price: number;
    effectiveDate: Date = new Date();
}

export class SalesPrice {
    id: number;
    productItem: ProductItem;
    price: number;
    effectiveDate: Date = new Date();
}

export class Product {
    id: number;
    name: string;
    description: string;
    img: ProductImage[];
    item: ProductItem[]; 
    category: ProductCategory;
    warrantyInformation: string;
}

export class ProductImage {
    id: string;
    name: string;
    urlImage: string;
}
