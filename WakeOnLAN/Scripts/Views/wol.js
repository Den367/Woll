$(document).ready(function() {
    $('#nameFilter').keyup(function(e) {
        clearTimeout($.data(this, 'timer'));
        if (e.keyCode == 13) {
           
            search(true);
        } else {
         
            $(this).data('timer', setTimeout(search, 500));
        }
    });

   
});