/*register email checker, if email is already in registered*/


document.getElementById("registrationForm").addEventListener("submit", async function (e) {
    e.preventDefault(); // Stop the form from moving to the next page immediately

    const emailValue = document.getElementById("emailInput").value;

    try {
        // 1. Use backticks (`) and standard routing: /Controller/Action?param=value
        const response = await fetch(`/Helper/GetUserEmailAsync?email=${encodeURIComponent(emailValue)}`);

        if (!response.ok)
        {
            console.error("Server error:", response.status);
            this.submit(); // Fallback: allow submission if the check fails
            return;
        }

        // 2. You must await the JSON conversion to get the true/false value
        const isEmailExists = await response.json();

        if (isEmailExists === true) {
            document.getElementById("emailErrorModal").style.display = "block";
        }
        else {
            // 3. Form is valid and unique. Call .submit() as a function.
            this.submit();
        }
    } catch (error) {
        console.error("Verification failed:", error);
        // Fallback: If the check fails, decide if you want to let them submit anyway
        // this.submit(); 
    }
});


/*close the email popup----------------------*/

function emailErrorModalClose()
{
    document.getElementById("emailErrorModal").style.display = "none";
}