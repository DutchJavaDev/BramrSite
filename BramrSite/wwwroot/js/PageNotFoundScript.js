export function startCountDown(url)
{
    setTimeout(function () {
        window.location.href = url;
    }, 10000);

    return "done";
}