jQuery(document).ready(function () {

   $("#menu-toggle").click(function(e) {
       e.preventDefault();
       $("#wrapper").toggleClass("toggled");
       $("#menu-toggle").children().toggleClass("glyphicon-chevron-left");
       $("#menu-toggle").children().toggleClass("glyphicon-chevron-right");
   });


   $('#StartTime').timepicker({ 'step': 15 });
   $('#EndTime').timepicker({ 'step': 15 });

    //Fall Back on Date if there is no native Date !!!//DONT REMOVE//!!!
   if(!Modernizr.inputtypes.date)
   {
       $('input[type=date]').datepicker({
           // Consistent format with the HTML5 picker
           dateFormat: 'yy-mm-dd'
       });       
   }
});