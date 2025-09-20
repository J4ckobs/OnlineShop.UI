document.addEventListener('DOMContentLoaded', function () {
    const validationSummary = document.querySelector('[asp-validation-summary]');
    if (validationSummary) {
        const errorMessages = validationSummary.querySelectorAll('li');
        if (errorMessages.length > 0) {
            validationSummary.classList.remove('is-hidden');
        }
    }

    const form = document.querySelector('form');
    const submitButton = form.querySelector('button[type="submit"]');
    form.addEventListener('submit', function () {
        submitButton.classList.add('is-loading');
        submitButton.disabled = true;
    });
});