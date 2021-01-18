export function Init()
{
    $('.menu-toggler').on('click', function () {
        $(this).toggleClass('open');
        $('.top-nav').toggleClass('open');
    });

    $('.top-nav .nav-link').on('click', function () {
        $('.menu-toggler').removeClass('open');
        $('.top-nav').removeClass('open');
    });

    $('.nav a[href*="#"]').on('click', function () {
        $('html, body').animate(keyframes, {
            scrollTop: $($(this).attr('href')).offset().top - 100
        }, Option(2000));
    });

    $(document).ready(function () {
        $(".draggable-item").draggable();
    });

    var modal = document.getElementById("editor");
    var btn = document.getElementById("myBtn");
    var span = document.getElementsByClassName("close")[0];

    if (btn != null)
    {
        btn.onclick = function () {
            modal.style.display = "block";
        }
    }

    if (span != null)
    {
        span.onclick = function () {
            modal.style.display = "none";
        }
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}
