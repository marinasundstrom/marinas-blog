export function init() {
    const offCanvasCollapse = document.querySelector('.offcanvas-collapse');

    // Toggle off canvas when clicking the menu toggle
    document.querySelector('#navbarSideCollapse').addEventListener('click', () => {
        offCanvasCollapse.classList.toggle('open');
    });

    // Toggle offcanvas off when clicking links
    document.querySelectorAll('a.nav-link')
        .forEach(linkElement => {
            if (linkElement.classList.contains("dropdown-toggle")) {
                // Toggle offcanvas when clicking the items, 
                // but ignore clicking the dropdown itself

                const linkElementParent = linkElement.parentElement;
                linkElementParent.querySelectorAll(".dropdown-item")
                    .forEach(itemElement => itemElement.addEventListener('click', () => {
                        offCanvasCollapse.classList.toggle('open');
                    }));
                return;
            }

            linkElement.addEventListener('click', () => {
                offCanvasCollapse.classList.toggle('open');
            })
        });

    const dropdown = new bootstrap.Dropdown(document.querySelector(".theme-selector .dropdown-toggle"));

    const elem = document.querySelector(".theme-selector");
    elem.addEventListener("click", () => {
        dropdown.toggle();
    });
}

export function initCM() {
    initColorModes();
}

export function scrollableNavbar() {
    const navbar = document.querySelector(".navbar");
    const navbarCollapse = document.querySelector(".navbar-collapse");

    const updateNavbarBackground = () => {
        if (window.scrollY > 1) {
            navbar.classList.add("shadow-sm");
            navbar.classList.add("bg-brand");
            navbarCollapse.classList.add("bg-brand");
            navbarCollapse.classList.remove("navbar-transparent");
        } else {
            navbar.classList.remove("shadow-sm");
            navbar.classList.remove("bg-brand");
            navbarCollapse.classList.remove("bg-brand");
            navbarCollapse.classList.add("navbar-transparent");
        }
    };

    window.addEventListener("scroll", updateNavbarBackground);
    window.addEventListener("DOMContentLoaded", updateNavbarBackground);

    updateNavbarBackground();
}