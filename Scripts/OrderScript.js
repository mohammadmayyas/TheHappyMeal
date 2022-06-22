
function CustomerCheck() {
    var CustomerName = $('#name').val();
    $.ajax({
        type: "POST",
        url: '/Orders/CheckCustomerName',
        data: '{username: "' + CustomerName + '"}',
        contentType: "application/json; charset=utf- 8",
        dataType: "json",
        success: function (result) {
            var mess = $("#status");
            if (result) {
                mess.html("Customer name not exist");
                mess.css("color", "red");
                $("#add").attr("disabled", true);
            }
            else {
                mess.html("Customer name exist");
                mess.css("color", "green");
                $("#add").attr("disabled", false);
            }
        }
    })
}

$(function () {
    $("#CatId").change(function () {
        $("#MealsList").children().remove();
        $("#MealsList").append("<option value='" + 0 + "'>" + "--Select Meal--" + "</option>");
        $.get("/Orders/GetMealsByCatyId", { ID: $("#CatId").val() }, function (data) {
            $.each(data, function (index, row) {
                $("#MealsList").append("<option value='" + row.MealId + "'>" + row.Name + "</option>")
            });
        })
    });
});

$(document).ready(function () {
    $("#MealsList").change(function () {
        var mealId = $("#MealsList").val();
        GetPriceByMealId(mealId);
    });
});

function GetPriceByMealId(mealId) {
    $.ajax({
        async: true,
        type: 'Get',
        dataType: 'JSON',
        contentType: "application/json; charset=utf- 8",
        data: { mealId: mealId },
        url: '/Orders/getPriceByMealId',
        success: function (data) {
            $("#price").val(parseFloat(data).toFixed(2));
        },
        error: function () {
            alert("There is a problem to get the meal price");
        }
    });

}

$(function () {
    $("#add").click(function () {
        var isValid = true;

        if (document.getElementById("name").val == "") {
            $('#name').siblings('span.error').text('Enter customer name!');
            isValid = false;
        }
        else {
            $('#name').siblings('span.error').text('');
        }

        if (document.getElementById("MealsList").selectedIndex == 0) {
            $('#MealsList').siblings('span.error').text('Please select meal');
            isValid = false;
        }
        else {
            $('#MealsList').siblings('span.error').text('');
        }

        if (!($("#quantity").val().trim() != '' && (parseInt($('#quantity').val()) || 0))) {
            $('#quantity').siblings('span.error').text('Please enter quantity');
            isValid = false;
        }
        else {
            $('#quantity').siblings('span.error').text('');
        }

        if (isValid) {

            var total = parseInt($("#quantity").val()) * parseFloat($("#price").val());
            $("#total").val(total);

            var totalPrice = parseFloat($("#total").val()) + parseFloat($("#totalPrice").val());
            $("#totalPrice").val(totalPrice);
            

            var MealID = document.getElementById("MealsList").value;

            var $newRow = $("#MainRow").clone().removeAttr('id');

            $('.MealsList', $newRow).val(MealID);

            $('#add', $newRow).addClass('remove').html('Remove').removeClass('btn-success').addClass('btn-danger');

            $('#MealsList, #quantity, #price', $newRow).attr('disabled', true);

            $("#MealsList, #quantity, #price, #total", $newRow).removeAttr("id");
            $("span.error", $newRow).remove();

            $("#OrderItems").append($newRow[0]);

            document.getElementById("MealsList").selectedIndex = 0;
            $("#price").val('');
            $("#quantity").val('');
            $("#total").val('');
        }
    });

    $("#OrderItems").on("click", ".remove", function () {
        var totalPrice;
        $("#OrderItems tr").each(function () {
            totalPrice = parseFloat($("#totalPrice").val()) - parseFloat($('.total', this).val());
        });
        $("#totalPrice").val(totalPrice);
        
        $(this).parents("tr").remove();
        
    });

    $("#submit").click(function () {
        var isValid = true;
        debugger;
        var itemsList = [];

        $("#OrderItems tr").each(function () {
            var item = {
                MealID: $('select.MealsList', this).val(),
                Price: $('.price', this).val(),
                Quantity: $('.quantity', this).val(),
                subTotal: $('.total', this).val()
            }
            itemsList.push(item);
        });

        if (itemsList.length == 0) {
            $('#orderMessage').text('Please add meals !');
            isValid = false;
        }

        if (isValid) {
            var data = {
                CustomerName: $("#name").val(),
                TotalPrice: $("#totalPrice").val(),
                Items: itemsList
            }

            $("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/Orders/AddOrderAndOrderDetials',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Submit');
                    }
                    else {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Submit');
                    }
                }
            });
        }
    });
});

//////////////////////////////////////////////////////////

$(function () {
    var count = 0;
    var Qval = 1;
    var totalPrice;
    $(".add").click(function () {
            if (totalPrice == 0) {
                count = 0;
            }
            
            var MealID = $(this).siblings('span').prop('id');
            var MealName = $(this).siblings('span').prop('title');
            var MealPrice = $(this).siblings('span').prop('class');
            var MealImg = $(this).siblings('span').prop('lang');
            var path = "../Uploads/Meals/";
        

            if (count == 0) {
                $("#customerOrders").append("<div class='order-details'></div>");

                $(".order-details").append("<button type='button' class='close' aria-label='Close'> <span aria-hidden='true'>&times;</span></button>");
                $(".order-details").append('<span class="mealName">' + MealName + '</span>');
                
                $(".order-details").append("<div class='mealImage' ></div>");
                $(".mealImage").append('<img src="' + path + MealImg + '" class="meal-img pic-blo3 size14 m-r-28 p-l-20">');

                $(".order-details").append('<input class="mealPrice" value=' + MealPrice + ' disabled/>');
                
                $(".order-details").append("<div class='Qunt'><div>");
               
                $(".Qunt").append("<input class='mealQnt' value='1' min='1' type='number' name='quantity'   '/>");
               
                $(".order-details").append("<input class='subTotal' disabled/>");
                $(".order-details").append("<input class='mealId' value=" + MealID + " style='visibility: hidden' disabled >");
               
                count++;
            }
            else if (count > 0) {
                var $newDiv = $(".order-details").clone();
                $('.mealName', $newDiv).text(MealName);
                $('.meal-img', $newDiv).attr("src", path + MealImg);
                $('.mealPrice', $newDiv).val(MealPrice);
                $('.mealId', $newDiv).val(MealID);

                $('.subTotal', $newDiv).replaceWith(subTotal);
                $('.mealQnt', $newDiv).val(1);

                $("#customerOrders").append($newDiv[0]);
            }
            
            var subTotal = parseInt(Qval) * parseFloat(MealPrice);
           
            totalPrice = parseFloat(subTotal) + parseFloat($("#orderTotal").val());
            $("#orderTotal").val(totalPrice);

    });

    $("#customerOrders").on("click", ".close", function () {
        $(this).parents(".order-details").remove();
        totalPrice = 0;

        $("#customerOrders .order-details").each(function () {
            var subTotal = $('.mealPrice', this).val() * $('.mealQnt', this).val();
            totalPrice = parseFloat(totalPrice) + parseFloat(subTotal);
        });

        $("#orderTotal").val(totalPrice);
    });

    $("#customerOrders").on("click", ".mealQnt", function () {
        totalPrice = 0;

        $("#customerOrders .order-details").each(function () {
            var subTotal = $('.mealPrice', this).val() * $('.mealQnt', this).val();
            totalPrice = parseFloat(totalPrice) + parseFloat(subTotal);
        });

        $("#orderTotal").val(totalPrice);
    });

    $("#submit").click(function () {
        var isValid = true;
        var itemsList = [];
        var subTotal;
        debugger;
        $("#customerOrders .order-details").each(function () {
            subTotal = $('.mealPrice', this).val() * $('.mealQnt', this).val();
            var item = {
                MealID: $('.mealId', this).val(),
                Price: $('.mealPrice', this).val(),
                Quantity: $('.mealQnt', this).val(),
                subTotal: subTotal
            }
            itemsList.push(item);
            
        });

        if (itemsList.length == 0) {
            $('#orderMessage').text('Please add meals !');
            isValid = false;
        }

        if (isValid) {
            var data = {
                CustomerName: $("#userName").val(),
                TotalPrice: $("#orderTotal").val(),
                Items: itemsList
            }

            $("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/Orders/AddOrderAndOrderDetials',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Submit');
                    }
                    else {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Submit');
                    }
                }
            });
        }
    });
});

//////////////////////////////////////////////////////////

$(function () {
    var count = 0;
    var Qval = 1;
    var totalPrice;
    $(".add2").click(function () {
        if (totalPrice == 0) {
            count = 0;
        }

        var OfferId = $(this).siblings('span').prop('id');
        var OfferName = $(this).siblings('span').prop('title');
        var OfferPrice = $(this).siblings('span').prop('class');
        var OfferImage = $(this).siblings('span').prop('lang');
        var path = "../Uploads/Offers/";


        if (count == 0) {
            $("#customerOrders2").append("<div class='order-details2'></div>");

            $(".order-details2").append("<button type='button' class='close2' aria-label='Close2'> <span aria-hidden='true'>&times;</span></button>");
            $(".order-details2").append('<span class="offerName">' + OfferName + '</span>');

            $(".order-details2").append("<div class='offerImage' ></div>");
            $(".offerImage").append('<img src="' + path + OfferImage + '" class="offer-img pic-blo3 size14 hov-img-zoom m-r-28 p-l-20">');

            $(".order-details2").append('<input class="offerPrice" value=' + OfferPrice + ' disabled/>');

            $(".order-details2").append("<div class='Qunt2'><div>");

            $(".Qunt2").append("<input class='offerQnt' value='1' min='1' type='number' name='quantity'   '/>");

            $(".order-details2").append("<input class='subTotal2' />");
            $(".order-details2").append("<input class='offerId' value=" + OfferId + " style='visibility: hidden' disabled >");

            count++;
        }
        else if (count > 0) {
            var $newDiv = $(".order-details2").clone();
            $('.offerName', $newDiv).text(OfferName);
            $('.offer-img', $newDiv).attr("src", path + OfferImage);
            $('.offerPrice', $newDiv).val(OfferPrice);
            $('.offerId', $newDiv).val(OfferId);

            $('.subTotal2', $newDiv).replaceWith(subTotal);
            $('.offerQnt', $newDiv).val(1);

            $("#customerOrders2").append($newDiv[0]);
        }

        var subTotal = parseInt(Qval) * parseFloat(OfferPrice);

        totalPrice = parseFloat(subTotal) + parseFloat($("#orderTotal2").val());
        $("#orderTotal2").val(totalPrice);

    });

    $("#customerOrders2").on("click", ".close2", function () {
        $(this).parents(".order-details2").remove();
        totalPrice = 0;

        $("#customerOrders2 .order-details2").each(function () {
            var subTotal = $('.offerPrice', this).val() * $('.offerQnt', this).val();
            totalPrice = parseFloat(totalPrice) + parseFloat(subTotal);
        });

        $("#orderTotal2").val(totalPrice);
    });

    $("#customerOrders2").on("click", ".offerQnt", function () {
        totalPrice = 0;

        $("#customerOrders2 .order-details2").each(function () {
            var subTotal = $('.offerPrice', this).val() * $('.offerQnt', this).val();
            totalPrice = parseFloat(totalPrice) + parseFloat(subTotal);
        });

        $("#orderTotal2").val(totalPrice);
    });

    $("#submit2").click(function () {
        var isValid = true;
        var itemsList = [];
        var subTotal;
        debugger;
        $("#customerOrders2 .order-details2").each(function () {
            subTotal = $('.offerPrice', this).val() * $('.offerQnt', this).val();
            var item = {
                MealID: $('.offerId', this).val(),
                Price: $('.offerPrice', this).val(),
                Quantity: $('.offerQnt', this).val(),
                subTotal: subTotal
            }
            itemsList.push(item);

        });

        if (itemsList.length == 0) {
            $('#orderMessage').text('Please add offers !');
            isValid = false;
        }

        if (isValid) {
            var data = {
                CustomerName: $("#userName").val(),
                TotalPrice: $("#orderTotal2").val(),
                Items: itemsList
            }

            $("#submit2").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/Orders/AddOrderAndOrderDetials',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#orderMessage').text(data.message);
                        $("#submit2").attr("disabled", false);
                        $("#submit2").html('Submit2');
                    }
                    else {
                        $('#orderMessage').text(data.message);
                        $("#submit2").attr("disabled", false);
                        $("#submit2").html('Submit2');
                    }
                }
            });
        }
    });
});

