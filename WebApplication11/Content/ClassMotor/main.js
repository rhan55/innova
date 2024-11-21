var ECS = {};

// ECS ozen Namespace and functions declaration.
ECS.ozen = {
    // Section starter method
    startSection: function ($section, $sender) {
        ECS.ozen.closeSection(true);

        setTimeout(function () {
            ECS.ozen.viewPortResize();
            $section.attr('data-last-class', $.getRandomEffect(1));
            $section.show();
            $section.addClass('active').fadeIn().addClass('animated ' + $section.attr('data-last-class'));

            $('.btn-pane').show();

            $('.cls-btn').click(function (e) {
                e.preventDefault();
				$('.main-menu').css('z-index','2225');
                ECS.ozen.closeSection(false);
            });

            if ($section.attr('data-downloadable') == "true") {
                $('*[data-d="true"]').show();
                $('.btn-pane a[data-d="true"]').attr('href', $section.attr('data-download-link'));
            } else {
                $('*[data-d="true"]').hide();
            }

            if ($section.find('iframe').length) {
                var $target = $section.find('iframe').first();
                $target.attr({
                    'src': $target.attr('data-src'),
                    'width': $target.attr('data-iwidth'),
                    'height': $target.attr('data-iheight')
                });
            }

            $('.bg-overlay').fadeIn();
            $('.shop-now-pane').fadeOut();

            $sender.addClass('active');

            if ($section.find('.bxslider').length > 0) {
                // ECS.ozen.catalogSlider.reloadSlider();
            };

            if ($section.find('.phillo-slide').length > 0) {
                ECS.ozen.philloSlider.reloadSlider({
                    pager: false,
                    useCss: false,
                    video: true,
                    onSliderLoad: function (currentIndex) {
                        $('.phillo-cont').find('div').css('visibility', 'hidden');
                        $('.phillo-slide li').not('.bx-clone').eq(currentIndex).find('.phillo-left').css('visibility', 'visible').addClass('animated slideInDown');
                        $('.phillo-slide li').not('.bx-clone').eq(currentIndex).find('.phillo-right').css('visibility', 'visible').hide().delay(1000).fadeIn(1500);
                    },
                    onSlideAfter: function ($slideElement, oldIndex, newIndex) {
                        //console.log($slideElement);
                        $('.phillo-slide li').not('.bx-clone').eq(oldIndex).find('.phillo-left').css('visibility', 'hidden').removeClass('animated slideInDown');
                        $('.phillo-slide li').not('.bx-clone').eq(oldIndex).find('.phillo-right').css('visibility', 'hidden').removeClass('animated slideInUp');
                        console.log($('.phillo-slide li').eq(oldIndex));
                        $slideElement.find('.phillo-left').css('visibility', 'visible').addClass('animated slideInDown');
                        $slideElement.find('.phillo-right').css('visibility', 'visible').hide().delay(1000).fadeIn(1500);
                    }
                });
            };
        }, 500);
    },

    // Section closer method
    closeSection: function (switching) {
        $('.btn-pane').hide();
        //if (isMobile()) {
        //    $('.menu-switcher').trigger('click');
        //}
        setTimeout(function () {
            var $section = $('.cata-inner:visible');
            $('.main-menu').find('a').removeClass('active');
            $section.removeClass($section.attr('data-last-class')).fadeOut(function () {
                if (!switching) {
                    $('.bg-overlay').fadeOut();
                    $('.shop-now-pane').fadeIn();
                }
                if ($section.find('iframe').length) {
                    var $target = $section.find('iframe').first();
                    $.each(['src', 'width', 'height'], function (i, attr) {
                        $target.removeAttr(attr);
                    });
                }
            });
        }, 600);
    }
    ,

    // Viewport resize method
    viewPortResize: function () {
        $('.bg-wrapper').find('img').css({
            width: '100%',
            height: '100%'
        });

        if ($('.bg-wrapper').find('img').height() < $(window).innerHeight()) {

            if ($('.bg-wrapper').find('img').width() < $(window).innerWidth()) {
                //console.log('Width');
                $('.bg-wrapper').find('li').not('.bx-clone').css({
                    'overflow': 'hidden'
                }).find('img').css({
                    'width': '100%',
                    'height': 'auto'
                });
            } else {
                //console.log('Height : ' + $(window).innerHeight());
                $('.bg-wrapper').find('li').not('.bx-clone').css({
                    'overflow': 'hidden'
                }).find('img').css({
                    'height': $(window).innerHeight(),
                    'max-width': 'none',
                    'float': 'right',
                    'width': 'auto'
                });
            }
        } else {
            $('.bg-wrapper').find('li').not('.bx-clone').css({
                'overflow': 'hidden'
            }).find('img').css({
                width: '100%',
                height: 'auto'
            });
        }

        var $paneWidth = $(window).innerWidth();

        $('iframe').each(function () {
            $(this).resizeAspected($paneWidth - 20);
        });

        if (isMobile()) {
            $('.content-inner').css('left', 0);
        } else {
            $('.content-inner').css('left', 300);
        }
		
		/*var _isMobile = window.matchMedia("only screen and (max-width: 760px)");
		
		if (_isMobile.matches) {
			
			$('.cata-inner.catalogue').css({
            'width': $paneWidth - 200 < 980 ? $paneWidth - 200 : 980,
            'margin-left': (($('.cata-inner.catalogue').width() + ($('.main-menu').width() + (isMobile() ? -500 : 0))) / 2) * -1 -100
		});
		
		}else{
        
		}*/
		
		$('.cata-inner.catalogue').css({
            'width': $paneWidth - 200 < 980 ? $paneWidth - 200 : 980,
            'margin-left': (($('.cata-inner.catalogue').width() + ($('.main-menu').width() + (isMobile() ? -680 : 0))) / 2) * -1 -200
		});
		
        // ECS.ozen.catalogSlider.reloadSlider({ pager: false, slideWidth: $paneWidth - 200 });
		
		/*if (_isMobile.matches) {
        setTimeout(function () {
            $('.phillo').css({
                'width': $paneWidth - 200 < 980 ? $paneWidth - 200 : 980,
                'margin-top': ($('.cata-inner.phillo').height() / 2) * -1 -100,
                'margin-left': (($('.cata-inner.phillo').width() + ($('.main-menu').width() + (isMobile() ? -300 : 0))) / 2) * -1 -10
            });
        }, 100);
		}else{
			
			
			
		}*/
		
		setTimeout(function () {
            $('.phillo').css({
                'width': $paneWidth - 200 < 980 ? $paneWidth - 200 : 980,
                'margin-top': ($('.cata-inner.phillo').height() / 2) * -1,
                'margin-left': (($('.cata-inner.phillo').width() + ($('.main-menu').width() + (isMobile() ? -400 : 0))) / 2) * -1 - 70
            });
        }, 100);
		
        var $safePaneWidth = $paneWidth - 100 < 980 ? $paneWidth - 100 : 980;

        $('.cata-inner.stores').css(
        {
            'max-width': '80%',
            
            'margin-left': ($('.cata-inner.stores').innerWidth() + $('.main-menu').width() + $('.main-menu').offset().left) / 2 * -1 + 100
        });
		
		
/*
			if (_isMobile.matches) {
				$('.cata-inner.contact').css(
				{
					'max-width': '80%',
					'margin-top': $('.cata-inner.contact').innerHeight() / 2 * -1,
					'margin-left': ($('.cata-inner.contact').innerWidth() + $('.main-menu').width() + $('.main-menu').offset().left) / 2 * -1 + 50
				});
			}else{
		
				
			}*/
			
			$('.cata-inner.contact').css(
				{
					'max-width': '80%',
					'margin-top': $('.cata-inner.contact').innerHeight() / 2 * -1,
					'margin-left': ($('.cata-inner.contact').innerWidth() + $('.main-menu').width() + $('.main-menu').offset().left) / 2 * -1 +30
				});
        //if (ECS.ozen.backgroundSlider) {
//            ECS.ozen.backgroundSlider.reloadSlider();
//        }
    }
    ,

    // Remove Start Loading
    removeLoading: function () {
        $('.load-overlay').fadeOut(500);
        $('.loading').remove();
    }
};

ECS.ozen.catalogSlider = null;
//ECS.ozen.backgroundSlider = null;
ECS.ozen.philloSlider = null;

$(document).ready(function () {
    $('.disabled-anchor').each(function () {
        $(this).click(function (e) {
            e.preventDefault();
        });
    });

   // ECS.ozen.catalogSlider = $('.bxslider').bxSlider({
//        slideWidth: (window.innerWidth - ($('.main-menu').width() + $('.main-menu').offset().left)) - 170,
//        pager: false
//    });

   // $('.menu-switcher').click(function (e) {
//        e.preventDefault();
//        if ($('.main-menu').css('left').split('p')[0] < 0) {
//            $(this).css('left', 310);
//            $('.main-menu').css('left', 0);
//        } else {
//            $('.main-menu').css('left', '-300px');
//            $(this).css('left', 10);
//        }
//    });
//
//    $('.menu-real').find('a').not('.skip').each(function () {
//        $(this).click(function (e) {
//            e.preventDefault();
//            var $sender = $(this);
//            var $section = $($sender.attr('data-sec'));
//			$('.main-menu').css('z-index','1');
//            ECS.ozen.startSection($section, $sender);
//        });
//    });

    $('.logo').click(function () {
        ECS.ozen.closeSection(false);
    });

    $('.country-list li').each(function () {
        $(this).click(function () {
            $(this).siblings().removeClass('active');
            $(this).addClass('active');
            var act = $(this);
            if (act.hasClass('has-province')) {
                $('.stores-list').hide();
                $('.stores-list li').removeClass('active');
                $('.province-list').hide();
                $('.province-list li').removeClass('active');
                $('.province-list[data-co=' + act.attr('data-co') + ']').slideDown(600, function () {
                    $('.province-list[data-co=' + act.attr('data-co') + ']').fadeIn(10);
                });
            } else {
                $('.stores-list').hide();
                $('.province-list').hide();
                $('.province-list li').removeClass('active');
                $('.stores-list[data-co=' + act.attr('data-co') + ']').fadeIn(600);
            }
        });
    });

    $('.province-list > li').each(function () {
        $(this).click(function () {
            var act = $(this);
            $(this).siblings().removeClass('active');
            $(this).addClass('active');
            $('.stores-list').fadeOut(600).delay(600).hide(function () {
                $('.stores-list[data-co=' + act.parent().attr('data-co') + '][data-id=' + act.attr('data-id') + ']').slideDown(600).fadeIn(10);
            });
        });
    });

    if (window.location.href == '#') {
        $('a[data-sec=".backstage"]').trigger('click');
    }
});

$(window).resize(function () {
    ECS.ozen.viewPortResize();
});

$(window).load(function () {
    //ECS.ozen.backgroundSlider = $('.bgxslider').bxSlider({
//        pager: false,
//        pagerType: 'short',
//        auto: true,
//		 controls: false,
//        pause: 5000,
//        speed: 1500,
//        onSlideBefore: function ($slideElement, oldIndex, newIndex) {
//            $('.bg-slider-nav ul').find('.active').removeClass('active'); $('.bg-slider-nav ul').find('li').eq(newIndex).addClass('active');
//        },
//        onSliderLoad: function (currentIndex) {
//            if ($('.bg-wrapper').find('img').height() < $(window).innerHeight()) {
//                $('.bg-wrapper .bx-viewport').css('height', $(window).innerHeight());
//                $('.bg-wrapper').find('li').not('.bx-clone').css({
//                    'overflow': 'hidden'
//                }).find('img').css({
//                    'height': $(window).innerHeight(),
//                    'max-width': 'none',
//                    'float': 'right',
//                    'width': 'auto'
//                });
//            } else {
//                $('.bg-wrapper .bx-viewport').css('height', 'auto');
//                $('.bg-wrapper').find('img').css({
//                    width: '100%',
//                    height: ($(window).height())+'px'
//                });
//            }
//            $('.bg-slider-nav ul').find('li').eq(currentIndex).trigger('click');
//        }
//    });
//
//    for (var i = 0; i < ECS.ozen.backgroundSlider.getSlideCount() ; i++) {
//        $('.bg-slider-nav').find('ul').append('<li>&nbsp;</li>');
//    }

    //$('.bg-slider-nav').find('ul li').first().addClass('active');
//
//    $('.bg-slider-nav').find('ul li').each(function () {
//        $(this).click(function () {
//            $(this).siblings('.active').removeClass('active');
//            $(this).addClass('active');
//            //ECS.ozen.backgroundSlider.goToSlide($(this).index());
//        });
//    });

//    ECS.ozen.philloSlider = $('.phillo-slide').bxSlider({
//        pager: false,
//        useCss: false,
//        video: true,
//        onSliderLoad: function (currentIndex) {
//            $('.phillo-cont').find('div').css('visibility', 'hidden');
//            $('.phillo-slide li').not('.bx-clone').eq(currentIndex).find('.phillo-left').css('visibility', 'visible').addClass('animated slideInDown');
//            $('.phillo-slide li').not('.bx-clone').eq(currentIndex).find('.phillo-right').css('visibility', 'visible').hide().delay(1000).fadeIn(1500);
//        },
//        onSlideAfter: function ($slideElement, oldIndex, newIndex) {
//            //console.log($slideElement);
//            $('.phillo-slide li').not('.bx-clone').eq(oldIndex).find('.phillo-left').css('visibility', 'hidden').removeClass('animated slideInDown');
//            $('.phillo-slide li').not('.bx-clone').eq(oldIndex).find('.phillo-right').css('visibility', 'hidden').removeClass('animated slideInUp');
//            console.log($('.phillo-slide li').eq(oldIndex));
//            $slideElement.find('.phillo-left').css('visibility', 'visible').addClass('animated slideInDown');
//            $slideElement.find('.phillo-right').css('visibility', 'visible').hide().delay(1000).fadeIn(1500);
//        }
//    });

    // ECS.ozen.catalogSlider.reloadSlider();

    ECS.ozen.viewPortResize();

    ECS.ozen.removeLoading();
});


// ECS.Core Implementation

function isMobile() {
    return $('.menu-switcher').is(':visible');
}

(function ($) {
    $.randomized = function () {
        return Math.random() * 300;
    };

    $.getRandomEffect = function (showOrHide) {
        var items = [];

        if (showOrHide > 0)
            items = ['bounceIn', 'bounceInRight', 'bounceInLeft', 'bounceInDown', 'bounceInUp', 'fadeIn', 'fadeInLeft', 'fadeInRight', 'fadeInUp', 'fadeInDown', 'slideInLeft', 'slideInUp', 'slideInDown', 'slideInRight'];
        else
            items = ['bounceOut', 'bounceOutRight', 'bounceOutLeft', 'bounceOutDown', 'bounceOutUp', 'fadeOut', 'fadeOutLeft', 'fadeOutRight', 'fadeOutUp', 'fadeOutDown', 'slideOutLeft', 'slideOutUp', 'slideOutDown', 'slideOutRight'];

        return items[Math.floor(Math.random() * items.length)];
    };

    $.fn.imagesLoaded = function () {

        var $imgs = this.find('img[src!=""]');
        // if there's no images, just return an already resolved promise
        var a = $.Deferred();
        if (!$imgs.length) {
            return a.resolve().promise();
        }

        // for each image, add a deferred object to the array which resolves when the image is loaded
        var dfds = [];
        $imgs.each(function () {

            var dfd = $.Deferred();
            dfds.push(dfd);
            var img = new Image();
            img.onload = function () { dfd.resolve(); };
            img.src = this.src;

        });

        // return a master promise object which will resolve when all the deferred objects have resolved
        // IE - when all the images are loaded
        return $.when.apply($, dfds);
    };

    $.fn.resizeAspected = function (iwidth) {
        var ratio = $(this).width() / $(this).height();
        var maxWidth = Number($(this).attr('data-iwidth')) || 980;
        var maxHeight = Number($(this).attr('data-iheight')) || 980;
        var iheight = iwidth / ratio;
        var mLeft = 0;

        if (maxWidth > iwidth && maxHeight > iheight) {

            mLeft = isMobile() ? (iwidth / 2) * -1 : (((iwidth + ($('.main-menu').width() + $('.main-menu').offset().left)) / 2) * -1) + 10;

            $(this).css({
                'width': iwidth - 20,
                'height': iheight,
                'max-width': maxWidth
            }).parent().css({
                'margin-left': mLeft,
                'margin-top': ((iheight / 2) * -1)
            });
        } else {

            mLeft = isMobile() ? (maxWidth / 2) * -1 : ((maxWidth + ($('.main-menu').width() + $('.main-menu').offset().left)) / 2) * -1;

            $(this).css({
                'width': maxWidth,
                'height': maxHeight,
                'max-width': maxWidth
            }).parent().css({
                'margin-left': mLeft,
                'margin-top': (maxHeight / 2) * -1
            });
        }
    };
    $.resizeBg = function (selector, iwidth, iheight, animate) {

        //$('#animLast img').not('.slogan > img')

        var asset = selector;
        var width = (iwidth != null && iwidth != 'undefined' && iwidth > 0) ? iwidth : $(window).innerWidth();
        var height = (iheight != null && iheight != 'undefined' && iheight > 0) ? iheight : $(window).innerHeight();

        var ratio = width / height;

        width = $(window).innerWidth();
        height = width / ratio;
        if (height < $(window).innerHeight() || $(window).innerHeight < height) {
            height = $(window).innerHeight();
            width = ratio * height;
        }

        //asset.animate({ 'width': width, 'height': height, 'left': '50%', 'margin-left': (width / 2) * -1 }, 800, 'easeOutBack', function () {
        asset.css({
            'width': width + 'px !important;',
            'height': height + 'px !important;',
            'left': '50%',
            'margin-left': ((width / 2) * -1) + ($('.main-menu').width() + $('.main-menu').offset().left)
        });
        //}); 
    }

})(jQuery);
