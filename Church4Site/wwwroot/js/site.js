// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




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