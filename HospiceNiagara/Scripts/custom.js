jQuery(document).ready(function () {

   

   $("#menu-toggle").click(function(e) {
       e.preventDefault();
       $("#wrapper").toggleClass("toggled");
       $("#menu-toggle").children().toggleClass("glyphicon-chevron-left");
       $("#menu-toggle").children().toggleClass("glyphicon-chevron-right");
   });
    
   $('#fileUpload').fileinput();

   $('#StartTime').timepicker({ 'step': 15 });
   $('#EndTime').timepicker({ 'step': 15 });

    //Fall Back on Date if there is no native Date !!!//DONT REMOVE//!!!
   if(!Modernizr.inputtypes.date)
   {
       $('input[type=date]').datepicker({
           // Consistent format with the HTML5 picker
           dateFormat: 'yy/mm/dd'
       });       
   }

   $('#incfont').click(function () {

       $('*').each(function () {
           $(this).css('font-size', parseInt($(this).css('font-size').replace(/px$/, '')) + 1 + "px");
       });
   });

   $('#decfont').click(function () {

       $('*').each(function () {
           $(this).css('font-size', parseInt($(this).css('font-size').replace(/px$/, '')) - 1 + "px");
       });
   });

    

});