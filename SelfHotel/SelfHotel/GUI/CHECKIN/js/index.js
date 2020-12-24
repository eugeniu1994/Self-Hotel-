
//jQuery time
var current_fs, next_fs, previous_fs; //fieldsets
var left, opacity, scale; //fieldset properties which we will animate
var animating; //flag to prevent quick multi-click glitches

function validateForm2(IdParinte) {
    var x, y, i, valid = true;
    var field = document.getElementById(IdParinte);
    y = field.querySelectorAll("[required]");
    for (i = 0; i < y.length; i++) {
        if (y[i].value == "") {
            y[i].className += " invalid";
            valid = false;
        } else {
            //y[i].className -= " invalid";
            y[i].classList.remove("invalid");
        }
    }
    return valid; // return the valid status
}

function nextPrev2(n,thing) {
    if (n > 0) {
        if (animating) return false;
        animating = true;

        current_fs = thing.parentNode;
        next_fs = thing.parentNode
        if (validateForm2(thing)) {
            $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

            //show the next fieldset
            next_fs.show();
            //hide the current fieldset with style
            current_fs.animate({ opacity: 0 }, {
                step: function (now, mx) {
                    //as the opacity of current_fs reduces to 0 - stored in "now"
                    //1. scale current_fs down to 80%
                    scale = 1 - (1 - now) * 0.2;
                    //2. bring next_fs from the right(50%)
                    left = (now * 50) + "%";
                    //3. increase opacity of next_fs to 1 as it moves in
                    opacity = 1 - now;
                    current_fs.css({
                        'transform': 'scale(' + scale + ')',
                        'position': 'absolute'
                    });
                    next_fs.css({ 'left': left, 'opacity': opacity });
                },
                duration: 800,
                complete: function () {
                    current_fs.hide();
                    animating = false;
                },
                //this comes from the custom easing plugin
                easing: 'easeInOutBack'
            });
        } else {

        }
    } else {

    }
}

$(".next").click(function(){
	current_fs = $(this).parent();
	next_fs = $(this).parent().next();
	var IdParinte = current_fs.attr('id');
	if (validateForm2(IdParinte)) {
	    if (animating) return false;
	    animating = true;
	    //verifica daca current_fs e completat tot, else afiseazai sa completeze tot
	    //activate next step on progressbar using the index of next_fs
	    $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

	    //show the next fieldset
	    next_fs.show();
	    //hide the current fieldset with style
	    current_fs.animate({ opacity: 0 }, {
	        step: function (now, mx) {
	            //as the opacity of current_fs reduces to 0 - stored in "now"
	            //1. scale current_fs down to 80%
	            scale = 1 - (1 - now) * 0.2;
	            //2. bring next_fs from the right(50%)
	            left = (now * 50) + "%";
	            //3. increase opacity of next_fs to 1 as it moves in
	            opacity = 1 - now;
	            current_fs.css({
	                'transform': 'scale(' + scale + ')',
	                'position': 'absolute'
	            });
	            next_fs.css({ 'left': left, 'opacity': opacity });
	        },
	        duration: 800,
	        complete: function () {
	            current_fs.hide();
	            animating = false;
	        },
	        //this comes from the custom easing plugin
	        easing: 'easeInOutBack'
	    });
	}
});

$(".previous").click(function(){
	if(animating) return false;
	animating = true;
	
	current_fs = $(this).parent();
	previous_fs = $(this).parent().prev();
	
	$("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");
	
	//show the previous fieldset
	previous_fs.show(); 
	//hide the current fieldset with style
	current_fs.animate({opacity: 0}, {
		step: function(now, mx) {
			//as the opacity of current_fs reduces to 0 - stored in "now"
			//1. scale previous_fs from 80% to 100%
			scale = 0.8 + (1 - now) * 0.2;
			//2. take current_fs to the right(50%) - from 0%
			left = ((1-now) * 50)+"%";
			//3. increase opacity of previous_fs to 1 as it moves in
			opacity = 1 - now;
			current_fs.css({'left': left});
			previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
		}, 
		duration: 800, 
		complete: function(){
			//current_fs.hide();
			animating = false;
		}, 
		//this comes from the custom easing plugin
		easing: 'easeInOutBack'
	});
});

//$(".submit").click(function(){
//	return false;
//})