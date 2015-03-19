jQuery(document).ready(function () {

   $("#menu-toggle").click(function(e) {
       e.preventDefault();
       $("#wrapper").toggleClass("toggled");
       $("#menu-toggle").children().toggleClass("glyphicon-chevron-left");
       $("#menu-toggle").children().toggleClass("glyphicon-chevron-right");
   });
});