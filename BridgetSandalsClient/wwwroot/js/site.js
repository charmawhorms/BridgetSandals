// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(document).ready(function () {
//    $('#carouselExampleSlidesOnly').carousel({
//        interval: 2000 // Sets the duration in milliseconds (2 seconds in this example)
//    });
//});


$('.carousel').carousel({
    interval: 2000
})

$('.scrolling-text-brands').slick({
    autoplay: true,
    autoplaySpeed: 2000,
    infinite: true,
    arrows: false,
    slidesToShow: 1,
    slidesToScroll: 1,
});

