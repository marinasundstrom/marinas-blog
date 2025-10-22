const listeners = new Map();
let generatedId = 0;

export function init(elementId = "TableOfContents") {
    // Delay the build very slightly so dynamically rendered markdown content is available in the DOM.
    const schedule = typeof window.requestAnimationFrame === "function"
        ? window.requestAnimationFrame.bind(window)
        : (callback) => window.setTimeout(callback, 0);

    schedule(() => buildToc(elementId));
}

export function dispose(elementId = "TableOfContents") {
    cleanup(elementId);
}

function buildToc(elementId) {
    cleanup(elementId);

    const tocContainer = document.getElementById(elementId);
    if (!tocContainer) {
        return;
    }

    const headings = Array.from(document.querySelectorAll("div.content section > h2, div.content section > h3"));
    if (headings.length === 0) {
        tocContainer.innerHTML = "";
        return;
    }

    const tocList = document.createElement("ul");
    const observedSections = [];
    let currentParentLi = null;

    headings.forEach((heading) => {
        const section = heading.closest("section");
        if (!section) {
            return;
        }

        const id = ensureSectionId(section, heading);
        if (!id) {
            return;
        }

        if (!observedSections.includes(section)) {
            observedSections.push(section);
        }

        const li = document.createElement("li");
        const link = document.createElement("a");
        link.href = `#${id}`;
        link.textContent = heading.textContent?.trim() ?? "";
        li.appendChild(link);

        if (heading.tagName.toUpperCase() === "H2") {
            tocList.appendChild(li);
            currentParentLi = li;
        } else {
            const parentLi = currentParentLi ?? tocList.lastElementChild;
            if (!parentLi) {
                tocList.appendChild(li);
                return;
            }

            let nestedList = parentLi.querySelector("ul");
            if (!nestedList) {
                nestedList = document.createElement("ul");
                parentLi.appendChild(nestedList);
            }

            nestedList.appendChild(li);
        }
    });

    tocContainer.innerHTML = "";
    tocContainer.appendChild(tocList);

    const update = () => updateActiveLink(observedSections);
    const throttledUpdate = throttle(update, 100);

    window.addEventListener("scroll", throttledUpdate, { passive: true });
    window.addEventListener("resize", throttledUpdate);
    window.addEventListener("hashchange", update);

    listeners.set(elementId, {
        handler: throttledUpdate,
        hashHandler: update
    });

    update();
}

function cleanup(elementId) {
    const existing = listeners.get(elementId);
    if (!existing) {
        return;
    }

    window.removeEventListener("scroll", existing.handler);
    window.removeEventListener("resize", existing.handler);
    window.removeEventListener("hashchange", existing.hashHandler);
    listeners.delete(elementId);
}

function updateActiveLink(sections) {
    if (!sections.length) {
        return;
    }

    const fromTop = window.scrollY + 120; // account for the fixed navbar height
    let currentSection = sections[0];

    sections.forEach((section) => {
        const sectionTop = section.getBoundingClientRect().top + window.scrollY;
        if (sectionTop <= fromTop) {
            currentSection = section;
        }
    });

    if (!currentSection || !currentSection.id) {
        return;
    }

    activateLinks(currentSection.id);
}

function activateLinks(id) {
    const selector = `nav.toc-nav a[href$="#${escapeSelector(id)}"]`;
    const matchingLinks = document.querySelectorAll(selector);

    if (matchingLinks.length === 0) {
        return;
    }

    document.querySelectorAll("nav.toc-nav a.active").forEach((link) => link.classList.remove("active"));

    matchingLinks.forEach((link) => {
        link.classList.add("active");
        const nav = link.closest("nav");
        if (!nav) {
            return;
        }

        if (nav.scrollHeight <= nav.clientHeight) {
            return;
        }

        const offsetTop = link.offsetTop;
        const offsetBottom = offsetTop + link.offsetHeight;
        if (offsetTop < nav.scrollTop) {
            nav.scrollTop = Math.max(offsetTop - nav.clientHeight * 0.3, 0);
        } else if (offsetBottom > nav.scrollTop + nav.clientHeight) {
            nav.scrollTop = offsetBottom - nav.clientHeight * 0.7;
        }
    });
}

function ensureSectionId(section, heading) {
    let id = sanitizeId(section.id);

    if (!id) {
        id = sanitizeId(heading.id) || sanitizeId(slugify(heading.textContent ?? ""));
    }

    if (!id) {
        id = `section-${generatedId += 1}`;
    }

    section.id = id;
    if (!heading.id) {
        heading.id = id;
    }

    return id;
}

function sanitizeId(id) {
    if (!id || typeof id !== "string") {
        return "";
    }

    return id
        .normalize("NFKD")
        .replace(/[\u0300-\u036f]/g, "")
        .replace(/"/g, "")
        .replace(/'/g, "")
        .replace(/`/g, "")
        .replace(/”/g, "")
        .replace(/“/g, "")
        .replace(/’/g, "")
        .replace(/‘/g, "")
        .replace(/</g, "")
        .replace(/>/g, "")
        .replace(/\(/g, "")
        .replace(/\)/g, "")
        .replace(/-&-/g, "-")
        .replace(/&/g, "")
        .replace(/!/g, "")
        .replace(/\?/g, "")
        .replace(/\s+/g, "-")
        .replace(/[^A-Za-z0-9_-]/g, "")
        .toLowerCase()
        .trim();
}

function slugify(value) {
    return value
        .toString()
        .trim()
        .toLowerCase()
        .replace(/&/g, "and")
        .replace(/[^a-z0-9\s-]/g, "")
        .replace(/\s+/g, "-")
        .replace(/-+/g, "-");
}

function escapeSelector(value) {
    if (typeof CSS !== "undefined" && typeof CSS.escape === "function") {
        return CSS.escape(value);
    }

    return value.replace(/([ #;?%&,.+*~\\':"!^$\[\]()=>|\/@])/g, "\\$1");
}

function throttle(fn, wait) {
    let timeoutId = null;
    let lastCallTime = 0;
    let trailingArgs = null;

    const invoke = (context, args) => {
        lastCallTime = Date.now();
        timeoutId = null;
        fn.apply(context, args);
    };

    return function throttled(...args) {
        const now = Date.now();
        const remaining = wait - (now - lastCallTime);
        trailingArgs = args;

        if (remaining <= 0 || remaining > wait) {
            if (timeoutId) {
                clearTimeout(timeoutId);
                timeoutId = null;
            }

            invoke(this, args);
        } else if (!timeoutId) {
            timeoutId = setTimeout(() => invoke(this, trailingArgs), remaining);
        }
    };
}
