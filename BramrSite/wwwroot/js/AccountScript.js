export function Init()
{
    $('.nav ul li').click(function () {
        $(this).addClass("active").siblings().removeClass('active');
    })
}

export function tabs(panelIndex) {
    const tab = document.querySelectorAll('.tab');

    tab.forEach(function (node) {
        node.style.display = 'none';
    });
    if(tab.length > 0)
    tab[panelIndex].style.display = 'block';
}