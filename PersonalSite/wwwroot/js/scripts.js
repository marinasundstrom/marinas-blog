"use strict";

window.blazorHelpers = {
    scrollToFragment: function (elementId) {
        var element = document.getElementById(elementId);
        if (element) {
            element.scrollIntoView({
                behavior: 'smooth'
            });
        }
    },
    enablePopovers: function() {
        var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
        var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
          return new bootstrap.Popover(popoverTriggerEl)
        })
    }
};
window.disqus = {
    initThread: function (config) {
        var d = document;
        var dsqThread = document.getElementById("dsq-thread");
        if (dsqThread) {
            (d.head || d.body).removeChild(dsqThread);
        }
        var d = document, s = d.createElement('script');
        s.id = "dsq-thread";
        s.src = "https://" + config.site + ".disqus.com/embed.js";
        s.setAttribute('data-timestamp', new Date());
        (d.head || d.body).appendChild(s);
    },
    initCount: function (config) {
        var d = document;
        var countScrTag = document.getElementById("dsq-count-scr");
        if (countScrTag) {
            d.body.removeChild(countScrTag);
        }
        var d = document, s = d.createElement('script');
        s.id = "dsq-count-scr";
        s.src = "https://" + config.site + ".disqus.com/count.js";
        //s.async = false;
        d.body.appendChild(s);
        if (typeof DISQUSWIDGETS != "undefined") {
            DISQUSWIDGETS.getCount({ reset: true });
        }
    }
};
var hljs;
window.setDocumentTitle = function (value) {
    document.title = value;
};
/*
// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction(); };
function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("scrollTopButton").style.display = "block";
    }
    else {
        document.getElementById("scrollTopButton").style.display = "none";
    }
}
*/

// When the user clicks on the button, scroll to the top of the document
window.topFunction = function () {
    window.scrollTo({
        top: 0,
        left: 0,
        behavior: 'smooth'
    });
};
window.initHighlight = function () {
    for (var _i = 0, _a = Array.from(document.getElementsByTagName("code")); _i < _a.length; _i++) {
        var element = _a[_i];
        hljs.highlightBlock(element);
        hljs.lineNumbersBlock(element);
    }
};
window.openInNewTab = function (href) {
    Object.assign(document.createElement('a'), {
        target: '_blank',
        href: href,
    }).click();
};

/*
const sections = document.querySelectorAll(".sections section");
const navLi = document.querySelectorAll(".sidenav ul li a");

window.addEventListener('scroll', () => {
    let current = "";
    Array.from(sections).reverse().forEach(section => {
        const sectionTop = section.offsetTop;
        const sectionHeight = section.clientHeight;

        var sectionA = Array.from(navLi).find(x => x.href && x.href.endsWith(`#${section.firstElementChild.id}`));

        if(pageYOffset > section.offsetTop && pageYOffset < section.offsetTop + section.clientHeight)
        {
            sectionA.classList.add("mystyle");
        }
        else 
        {
            sectionA.classList.remove("mystyle");
        }
    });
});
*/

window.isDarkMode = () => {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        return true;
    }
    return false;
};

window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', async event => {
    if(event.matches)
        document.body.classList.add("dark");
    else
        document.body.classList.remove("dark");

    await DotNet.invokeMethodAsync("PersonalSite", "OnDarkModeChanged", event.matches);
});

window.setColorScheme= (colorScheme) => {
    if(colorScheme === "dark")
        document.body.classList.add("dark", "text-white", "bg-dark");
    else
        document.body.classList.remove("dark", "text-white", "bg-dark");
};

function splashscreen() {
    let preferredColorScheme = JSON.parse(window.localStorage["preferredColorScheme"] ?? "null");
    let colorScheme = preferredColorScheme ?? (window.isDarkMode() ? 1 : 0);

    if (colorScheme == 1) {
        const elem = document.getElementById("splashscreen");
        //elem.classList.toggle("dark");
    }
}

splashscreen();

function getScrollParent(element, includeHidden) {
    var style = getComputedStyle(element);
    var excludeStaticParent = style.position === "absolute";
    var overflowRegex = includeHidden ? /(auto|scroll|hidden)/ : /(auto|scroll)/;

    if (style.position === "fixed") return document.body;
    for (var parent = element; (parent = parent.parentElement);) {
        style = getComputedStyle(parent);
        if (excludeStaticParent && style.position === "static") {
            continue;
        }
        if (overflowRegex.test(style.overflow + style.overflowY + style.overflowX)) return parent;
    }

    return document.body;
}

window.navbarInit = function() {
    const navbar = document.getElementsByClassName("navbar").item(0);
    const app = document.getElementsByTagName("h1").item(0);

    let p = getScrollParent(app);

    p.addEventListener("scroll", (ev2) => {
        if (document.body.scrollTop > 20) {
            navbar.classList.add("bg-brand");
        }else{
            navbar.classList.remove("bg-brand");
        }
    });

    const el = document.querySelector(".bgimg-1");

    el.addEventListener("mousemove", (e) => {
        let source = e.target;

        if(source.classList.contains("rounded-circle") || source.classList.contains("btn")) {
            e.stopPropagation();
            return;
        }

        el.style.backgroundPositionX = (-e.offsetX * 0.2) + "px";
        //el.style.backgroundPositionY = (-e.offsetY * 0.2) + "px";
    });

    var observer = new IntersectionObserver(function(entries) {
        if(entries[0].isIntersecting === true)
            console.log('Element is fully visible in screen');
    }, { threshold: [0.8] });
    
    observer.observe(document.querySelector(".context"));
};
