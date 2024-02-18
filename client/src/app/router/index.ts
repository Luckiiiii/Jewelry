import { RouterModule } from "@angular/router";
import { CheckoutPage } from "../pages/checkout.component";
import { ShopPage } from "../pages/shopPage.component";
import { LoginPage } from "../pages/loginPage.component";
import { AuthActivator } from "../services/authActivator.service";
import ProductDetail from "../pages/productDetail.component";

const routes = [
    { path: "", component: ShopPage },
    { path: "checkout", component: CheckoutPage, canActivate: [AuthActivator] },
    { path: "login", component: LoginPage },
    { path: "product/:id", component: ProductDetail }, 
    { path: "**", redirectTo: "/"}
];

const router = RouterModule.forRoot(routes, {
    useHash: false
});

export default router;