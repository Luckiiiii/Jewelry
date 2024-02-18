$(document).ready(function () {
    var theForm = $("#theForm");
    theForm.hide();

    var button = $("#buyButton");
    button.on("click", function () {
        console.log("Buying item");
    });

    var productInfo = $(".product-props li");
    //var listItems = productInfo.item[0].children;
    productInfo.on("click", function () {
        console.log("you clicked on" + $(this).text());
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $("#popup-form");

    $loginToggle.on("click", function () {
        $popupForm.slideToggle(1000);
    });
});
