export class ProductItem {
    id: number;
    product: Product;
    size: Size;
    price: number;
    quantity: number;
}

export class ProductPortfolio {
    id: number;
    name: string;
}

export class Size {
    id: number;
    name: string;
}

export class Product {
    id: number;
    category: string;
    title: string;
    description: string;
    artId: string;
    img: ProductImage[];
    portfolio: ProductPortfolio;
}

export class ProductImage {
    id: string;
    name: string;
    urlImage: string;
}
