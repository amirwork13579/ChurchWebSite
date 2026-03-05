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

    const formData = new FormData(form);

    try {
        const responce = await fetch('/Main/ContactFormSubmit', { method: 'POSt', body: formData });
        form.reset();

    }
    catch(error) {console.error("submit failed", error) }
}


async function submitFormAsync1() {
    const form = document.getElementById('churchContactForm');
    const wrapper = document.getElementById('form-wrapper');
    const spinner = document.getElementById('loading-spinner');

    // 1. Show a loading state (instantly changes UI)
    form.style.opacity = "0.5";
    spinner.style.display = "block";

    // 2. Gather the form data
    const formData = new FormData(form);

    try {
        // 3. Post to the Controller
        const response = await fetch('/Home/SubmitContactForm', {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            // 4. Get the "Success" HTML from the Partial View
            const resultHtml = await response.text();

            // 5. Replace the entire form with the success message
            wrapper.innerHTML = resultHtml;

            // 6. Change CSS to make it pop
            wrapper.style.backgroundColor = "#e7f3ef";
            wrapper.style.padding = "20px";
        } else {
            alert("Error saving to database.");
        }
    } catch (error) {
        console.error("Submission failed", error);
    } finally {
        spinner.style.display = "none";
    }
}







