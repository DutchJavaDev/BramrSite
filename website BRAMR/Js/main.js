/*  INLOGSYSTEEM */

const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');

signUpButton.addEventListener('click', () => {
	container.classList.add("right-panel-active");
});

signInButton.addEventListener('click', () => {
	container.classList.remove("right-panel-active");
});


/* POP UP MELDING */

document.getElementById("Open-PopUp-btn").addEventListener("click", function(){
    document.getElementsByClassName("PopUp")[0].classList.add("active");
});

document.getElementById("SluitAf-PopUp-btn").addEventListener("click", function(){
    document.getElementsByClassName("PopUp")[0].classList.remove("active");
});

