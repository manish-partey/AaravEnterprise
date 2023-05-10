!function ($) {
    'use strict';

    // write code here
    $('.slider').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        infinite: true,
        arrows: false,
        dots: true,
      });

    

      $('.menu-icon').click(function(){
        $('.navigation-wrapper').toggleClass('active');
        
      })

      $('.tab-link').click(function () {

        var tabID = $(this).attr('data-tab');
    
        $(this).addClass('active').siblings().removeClass('active');
    
        $('#tab-' + tabID).addClass('active').siblings().removeClass('active');
      });
      
}.call(window, window.jQuery);