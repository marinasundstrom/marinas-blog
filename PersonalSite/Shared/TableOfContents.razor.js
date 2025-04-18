export function init() {
    createTable();

    // simple function to use for callback in the intersection observer
    const changeNav = (entries, observer) => {
        // if(!entries[0].isIntersecting) return;

        entries.map(entry => {
            if (entry.isIntersecting) {
                // remove old active class
                document.querySelectorAll('.active').forEach(el => el.classList.remove('active'));

                // get id of the intersecting section
                var id = entry.target.getAttribute('id');

                // find matching link & add appropriate class

                let menuLink = document.querySelector(`[href$="${window.location.pathname}#${id}"]`);
                if (menuLink) {
                    menuLink.classList.add("active");
                }
            }
        })
    }

    // init the observer
    const options = {
        rootMargin: '0px',
        threshold: 1.0
    }

    const observer = new IntersectionObserver(changeNav, options);

    // target the elements to be observed
    const sections = document.querySelectorAll('.content section');
    sections.forEach((section) => {
        observer.observe(section);
    });
}

function createTable() {
    const tocContainer = document.getElementById("TableOfContents");
    if (!tocContainer) return;

    const headings = document.querySelectorAll(".content h2, .content h3"); //, .content h4, .content h5, .content h6");
    const tocList = document.createElement("ul");
    let lastLevels = [tocList];

    headings.forEach((heading, i) => {

        //if (i == 0) return;

        if (!heading.id) {
            heading.id = heading.textContent.trim().toLowerCase().replace(/\s+/g, '-');
        }

        const level = parseInt(heading.tagName.substring(1), 10);
        const text = heading.textContent;

        const id = sanitizeId(heading.id);

        const href = `#${id.replace('"', "")}`;

        const li = document.createElement("li");
        const a = document.createElement("a");
        a.href = window.location.pathname + href;
        a.textContent = text;
        li.appendChild(a);

        // Ensure the list is deep enough
        while (lastLevels.length < level) {
            const ul = document.createElement("ul");
            const lastLi = lastLevels[lastLevels.length - 1].lastElementChild;
            if (lastLi) {
                lastLi.appendChild(ul);
                lastLevels.push(ul);
            } else {
                // Create dummy li if needed (shouldn't normally happen)
                const dummyLi = document.createElement("li");
                lastLevels[lastLevels.length - 1].appendChild(dummyLi);
                dummyLi.appendChild(ul);
                lastLevels.push(ul);
            }
        }

        // Trim deeper levels if we're going back up
        lastLevels = lastLevels.slice(0, level);
        lastLevels[level - 1].appendChild(li);
    });

    tocContainer.appendChild(tocList);
}

function sanitizeId(id) {
    if (!id || id.trim() === "") {
        return "";
    }

    return id
        .replace(/"/g, "")
        .replace(/'/g, "")
        .replace(/`/g, "")
        .replace(/”/g, "")
        .replace(/“/g, "")
        .replace(/’/g, "")
        .replace(/‘/g, "")
        .replace(/</g, "")
        .replace(/>/g, "")
        .replace(/&/g, "")
        .replace(/!/g, "")
        .replace(/\?/g, "")
        .trim();
}