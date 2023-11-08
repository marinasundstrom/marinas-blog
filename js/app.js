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

window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', async event => {
   window.setColorScheme(event.matches ? "dark" : "light");
});

window.setColorScheme = (colorScheme) => {
    var linkElem = document.head.querySelector('link[href^="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.5.0/styles"]');

    if(linkElem == null) return;

    if(colorScheme === "dark")
    {
        linkElem.href = linkElem.href.replace("vs.min.css", "vs2015.min.css");
    }
    else 
    {
        linkElem.href = linkElem.href.replace("vs2015.min.css", "vs.min.css");
    }
};

const isDarkMode = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
window.setColorScheme(isDarkMode ? "dark" : "light");
