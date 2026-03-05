// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.





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








/*email checker-------------------------*/

/*
document.querySelector(".eventpost-form").addEventListener("submit", async function (e) {
    e.preventDefault(); // stop normal submit

    const email = document.getElementById("emailInput").value;

    // Send email to server
    const response = await fetch("/YourController/CheckEmail", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": document.querySelector("input[name='__RequestVerificationToken']").value
        },
        body: JSON.stringify({ email: email })
    });

    const result = await response.json();

    if (!result.exists) {
        alert("The email you entered does not exist.");
        return; // stop here, do NOT submit form
    }

    // Email exists → submit form normally
    e.target.submit();
});
*/



//page update no reload for contactForm
async function submitFormAsync() {
    const form = document.getElementById('churchContactForm');

    if (!form.checkValidity())
    {
        form.reportValidity();
        return;
    }

    const formData = new FormData(form);

    try {
        const responce = await fetch('/Main/ContactFormSubmit', { method: 'POSt', body: formData });
        if (responce.ok) {
            form.reset();
            alert("Your message has been sent successfully! We will get back to you as soon as possible.");
        }
        //else { alert("must fill in all feilds before submitting")}

    }
    catch(error) {console.error("submit failed", error) }
}







