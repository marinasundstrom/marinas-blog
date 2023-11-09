function initScrollToTop() {
    var scrollToTop = document.querySelector("#scrollToTop");
    
    if(!scrollToTop) return;

    scrollToTop.addEventListener("click", ev => {
        ev.preventDefault();
        
        scrollToTop();
    });
}

function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

initScrollToTop();