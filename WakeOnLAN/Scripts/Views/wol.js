$(document).ready(function () {
    var canDetect = "onorientationchange" in window;
    var orientationTimer = 0;
    var ROTATION_CLASSES = {
        "0": "none",
        "90": "right",
        "-90": "left",
        "180": "flipped"
    };
    
    $(window).bind(canDetect ? "orientationchange" : "resize"
    , function(evt) {
clearTimeout(orientationTimer);
    orientationTimer = setTimeout(function() {
      
        // calculate the orientation based on aspect ratio
        var aspectRatio = 1;
        if (window.innerHeight !== 0) {
            aspectRatio = window.innerWidth / window.innerHeight;
        } // if
        // determine the orientation based on aspect ratio
        var orientation = aspectRatio <= 1 ? "portrait" : "landscape";
        // if the event type is an orientation change event, we can rely on
        // the orientation angle
        var rotationText = null;
        if (evt.type == "orientationchange") {
            rotationText = ROTATION_CLASSES[window.orientation.toString()];
            alert(evt.type);
            search(true);
        } // if
        
    }, 500);
        
    });
    

    $('#nameFilter').keyup(function(e) {
        clearTimeout($.data(this, 'timer'));
        if (e.keyCode == 13) {
           
            search(true);
        } else {
         
            $(this).data('timer', setTimeout(search, 500));
        }
    });

   
});

