var autoScrollInterval = 1500;

$(document).ready(function () {
    // Activate carousel
    $('.carousel').carousel();

    // Start autoscroll
    setInterval(function () {
        $('.carousel').carousel('next');
    }, autoScrollInterval);
});