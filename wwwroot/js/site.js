// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    var div = $("#alert").fadeIn(3000)
        
    $(".alert").on('click', '.close', function (event) {
        let div = $(this).closest('div');
        div.fadeOut('3000', function() { div.css('display', 'none') });
    });
});
