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

   $('#incfont').click(function(){   
       resize(2);
   }); 
   $('#decfont').click(function(){   
       resize(-2);
   }); 

   var size = 0;

   function resize(y)
   {
       var y = size + y;
       //because....
       var size = y;
      
       h1= parseInt($('h1').css('font-size')) + y;
           $('h1').css('font-size', h1);
   
      h2 = parseInt($('h2').css('font-size')) + y;
           $('h2').css('font-size', h2);
 
       h3 = parseInt($('h3').css('font-size')) + y;
           $('h3').css('font-size', h3);

       h4 = parseInt($('h4').css('font-size')) + y;
           $('h4').css('font-size', h4);

       h5 = parseInt($('h5').css('font-size')) + y;
           $('h5').css('font-size', h5);

       h1 = parseInt($('h6').css('font-size')) + y;
           $('h6').css('font-size', h6);

       div = parseInt($('div').css('font-size')) + y;
           $('div').css('font-size', div);


       label = parseInt($('label').css('font-size')) + y;
           $('label').css('font-size', label);

       span = parseInt($('span').css('font-size')) + y;
           $('div').css('font-size', span);


      p = parseInt($('p').css('font-size')) + y;
           $('p').css('font-size', p);

       a = parseInt($('a').css('font-size')) + y;
           $('a').css('font-size', a);

      html = parseInt($('html').css('font-size')) + y;
           $('html').css('font-size', html);
   }
});