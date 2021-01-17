export function Init()
{
    $(document).ready(function () {
        $(".draggable-item").draggable();
    });
    //let script = document.createElement("script");

    //script.setAttribute('src', 'https://code.jquery.com/jquery-3.5.1.js');
    //script.setAttribute('integrity', 'sha256-QWo7LDvxbWT2tbbQ97B53yJnYU3WhH/C8ycbRAkjPDc=');
    //script.setAttribute('crossorigin', 'anonymous');

    //document.body.appendChild(script);

    var modal = document.getElementById("editor");
    var btn = document.getElementById("myBtn");
    var span = document.getElementsByClassName("close")[0];

    btn.onclick = function () {
        modal.style.display = "block";
    }

    span.onclick = function () {
        modal.style.display = "none";
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}
