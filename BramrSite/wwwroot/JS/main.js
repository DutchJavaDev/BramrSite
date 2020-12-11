document.getElementById("Open-PopUp-btn").addEventListener("click", function () {
    document.getElementsByClassName("PopUp")[0].classList.add("active");
});

document.getElementById("SluitAf-PopUp-btn").addEventListener("click", function () {
    document.getElementsByClassName("PopUp")[0].classList.remove("active");
});
