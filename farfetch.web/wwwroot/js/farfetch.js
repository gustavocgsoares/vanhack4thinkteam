/**
 * This class implements all other domain classes
 */

$(function () {
    
    //Customer operations
    //----------------------------------//

    $('form[name=checkout-form]').submit(function (e) {
        e.preventDefault

        var newCustomer = {

            firstname: $('input[name=co_f_name]').val(),
            lastname: $('input[name=co_l_name]').val(),
            email: $('input[name=co_email]').val(),
            phone: $('input[name=co_phone]').val(),
            shippingAddress: $('input[name=co_address1]').val() + $('input[name=co_address2]').val(),
            country: $('select[name=co_country]').val(),
            state: $('select[name=co_state]').val(),
            city: $('select[name=co_city]').val(),
            zip: $('input[name=co_zip]').val(),
        }

        customer.createCustomer(newCustomer);

    })

    //cart operations
    //----------------------------------//
    $('form[name=add-to-cart]').submit(function (e) {
        e.preventDefault();

        var itemToAdd = {
            //TODO: populate item
        }

        customer.addToCart(itemToAdd);
    })
})