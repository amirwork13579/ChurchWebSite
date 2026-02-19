/*hamburger menu js------------*/

document.addEventListener('DOMContentLoaded', () => {
    const trigger = document.querySelector('#portal-toggle');
    const wrapper = document.querySelector('.portal-wrapper');

    if (trigger && wrapper) {
        trigger.addEventListener('click', () => {
            // Toggles the hamburger icon state
            trigger.classList.toggle('is-open');
            // Toggles the visibility of the menu overlay
            wrapper.classList.toggle('is-visible');
        });

        // Close menu if a user clicks a link (helpful for one-page sites)
        wrapper.querySelectorAll('a').forEach(item => {
            item.addEventListener('click', () => {
                trigger.classList.remove('is-open');
                wrapper.classList.remove('is-visible');
            });
        });
    }
});




/*new hamburger nav*/

document.addEventListener('DOMContentLoaded', () => {
    const hamburger = document.getElementById('navbar-hamburger');
    const menu = document.getElementById('navbar-menu');

    // Selecting both regular links and the button
    const allLinks = document.querySelectorAll('.seethronavbar-link, .seethronavbar-btn');

    // Safety check to ensure elements exist
    if (hamburger && menu) {
        // Toggle mobile menu
        hamburger.addEventListener('click', (e) => {
            e.stopPropagation(); // Prevents click from bubbling
            hamburger.classList.toggle('is-active');
            menu.classList.toggle('is-active');
        });

        // Close menu when clicking any link
        allLinks.forEach(link => {
            link.addEventListener('click', () => {
                hamburger.classList.remove('is-active');
                menu.classList.remove('is-active');
            });
        });

        // Optional: Close menu if clicking outside the navbar
        document.addEventListener('click', (e) => {
            if (!menu.contains(e.target) && !hamburger.contains(e.target)) {
                hamburger.classList.remove('is-active');
                menu.classList.remove('is-active');
            }
        });
    }
});



document.addEventListener('DOMContentLoaded', () => {
    const hamburger = document.getElementById('navbar-hamburger');
    const navMenu = document.getElementById('navbar-menu');

    if (hamburger && navMenu) {
        hamburger.addEventListener('click', () => {
            // Toggle the 'active' class on both elements
            hamburger.classList.toggle('active');
            navMenu.classList.toggle('active');

            // Accessibility: Update aria-expanded
            const isExpanded = hamburger.classList.contains('active');
            hamburger.setAttribute('aria-expanded', isExpanded);
        });

        // Optional: Close menu when a link is clicked (useful for mobile)
        document.querySelectorAll('.seethronavbar-link').forEach(link => {
            link.addEventListener('click', () => {
                hamburger.classList.remove('active');
                navMenu.classList.remove('active');
            });
        });
    }
});








/*delete popup js----------------*/

document.addEventListener('click', function (e) {
    const trigger = e.target.closest('.deletepopupbutton-trigger');
    const overlay = document.getElementById('deletepopupbutton-overlay');
    const form = document.getElementById('deletepopupbutton-form');
    const hiddenInput = document.getElementById('deletepopupbutton-hidden-id');
    const idDisplay = document.getElementById('deletepopupbutton-id-display');

    // 1. When the trigger is clicked
    if (trigger) {
        const itemId = trigger.dataset.id;
        const actionUrl = trigger.dataset.actionurl;

        // Set form attributes dynamically
        form.action = actionUrl;
        hiddenInput.value = itemId;
        idDisplay.textContent = `#${itemId}`;

        // Show popup
        overlay.style.display = 'flex';
    }

    // 2. Close logic (Cancel button or clicking outside the card)
    if (e.target.id === 'deletepopupbutton-cancel' || e.target === overlay) {
        overlay.style.display = 'none';
    }
});





/*testimonie post popup----------------------------------*/

/* 1. The Interceptor: This stops the form and shows the popup */
document.getElementById("postForm").addEventListener("submit", function (e) {
    // Only stop it if the popup hasn't been acknowledged yet
    if (document.getElementById("testimoniepopup-overlay").style.display !== "flex") {
        e.preventDefault();
        openTestimoniePopup();
    }
});

/* 2. The Opener */
function openTestimoniePopup() {
    // Use "flex" to keep your centering logic working
    document.getElementById("testimoniepopup-overlay").style.display = "flex";
}

/* 3. The Closer & Submitter */
function closeTestimoniePopup() {
    // First, hide the popup
    document.getElementById("testimoniepopup-overlay").style.display = "none";

    // Second, manually tell the form to submit to the C# Controller
    document.getElementById("postForm").submit();
}





/*device open image upload*/
//my js fun to open device files and upload the image
function openDeviceFile(element, srcid) {
    const photo = element.files[0];

    const imgshower = document.getElementById(srcid);

    const imgfinal = URL.createObjectURL(photo);
    imgshower.src = imgfinal;
}









const dropZone = document.querySelector('.testimonieformpost-file-label');
const fileInput = document.getElementById('testimonyImage');

// Add 'active' class when dragging over
['dragenter', 'dragover'].forEach(eventName => {
    fileInput.addEventListener(eventName, () => {
        dropZone.style.borderColor = "#c0c0c0";
        dropZone.style.background = "rgba(255, 255, 255, 0.1)";
    }, false);
});

// Remove 'active' class when dragging leaves
['dragleave', 'drop'].forEach(eventName => {
    fileInput.addEventListener(eventName, () => {
        dropZone.style.borderColor = "rgba(255, 255, 255, 0.15)";
        dropZone.style.background = "transparent";
    }, false);
});