export function updateContainer() {
    const signUpButton = document.getElementById('signUp');
    const signInButton = document.getElementById('signIn');
    const container = document.getElementById('container');

    signUpButton.addEventListener('click', () => {
        container.classList.add("right-panel-active");
    });

    signInButton.addEventListener('click', () => {
        container.classList.remove("right-panel-active");
    });


    var tl = gsap.timeline({ defaults: { duration: .7, ease: Back.easeOut.config(2), opacity: 0 } })
    var tl2 = gsap.timeline({ defaults: { duration: 1.5, delay: 1 } })

    tl.from("#card-bg", { delay: 1, scale: .2, transformOrigin: 'center' }, "=.2")
        .from("#card-top", { scaleY: 0, transformOrigin: 'top' })
        .from("#icon", { scale: .2, transformOrigin: 'center' }, "-=.7")
        .from("#blip1", { scaleX: 0 })
        .from("#blip2", { scaleX: 0 }, "-=.2")
        .from("#blip3", { scaleX: 0 }, "-=.3")
        .from("#blip4", { scaleX: 0 }, "-=.5")
        .from("#blip5", { scaleX: 0 }, "-=.7")

    tl2.to(".whole-card", { y: 10, repeat: -1, yoyo: true })
};

export function scrollToId(elementId)
{
    var element = document.getElementById(elementId);

    if (!element) {
        console.warn('element was not found', elementId);
        return;
    }

    element.preventDefault();

    element.scrollIntoView();
}