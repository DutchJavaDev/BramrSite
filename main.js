/* POP UP MELDING */

document.getElementById("Open-PopUp-btn").addEventListener("click", function(){
    document.getElementsByClassName("PopUp")[0].classList.add("active");
});

document.getElementById("SluitAf-PopUp-btn").addEventListener("click", function(){
    document.getElementsByClassName("PopUp")[0].classList.remove("active");
});

/*  INLOGSYSTEEM */

var x = document.getElementById("inloggen");
var y = document.getElementById("registreren");
var z = document.getElementById("btndesign");

function registreren(){
    x.style.left = "-400px";
    y.style.left = "50px";
    z.style.left = "110px";
}
function inloggen(){
    x.style.left = "50px";
    y.style.left = "450px";
    z.style.left = "0px";
}