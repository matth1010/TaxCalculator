    const scriptTag = document.querySelector('script[src="js/tax-calculate-module.js"]');
    const _apiBaseUrl = scriptTag.getAttribute('data-api-base-url');

    function displayErrorMessage(message) {
        const errorContainer = document.getElementById("errorContainer");
        const errorMessage = document.getElementById("errorMessage");

        errorMessage.textContent = message;
        errorContainer.style.display = "block";
    }

    function calculateTax() {
        const postalCodeField = document.getElementById("postalCode");
        const annualIncomeField = document.getElementById("annualIncome");

        const postalCode = postalCodeField.value;
        const annualIncome = annualIncomeField.value;

        postalCodeField.classList.remove("validation-error");
        annualIncomeField.classList.remove("validation-error");

        if (!postalCode) {
            postalCodeField.classList.add("validation-error");
            displayErrorMessage("Please enter a valid postal code");
            return;
        }
        if (!annualIncome || isNaN(annualIncome)) {
            annualIncomeField.classList.add("validation-error");
            displayErrorMessage("Please enter a valid annual income.");
            return;
        }

        const apiEndpoint = `${_apiBaseUrl}/api/tax/calculate`;

        const taxCalculation = {
            postalCode: postalCode,
            annualIncome: parseFloat(annualIncome),
        };

        fetch(apiEndpoint, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(taxCalculation),
        })
            .then((response) => {
                if (response.ok) {
                    location.reload();
                } else if (response.status === 400) {
                    response
                        .json()
                        .then((data) => {
                            const errorMessage = data.reason || "An error occurred while calculating tax.";
                            displayErrorMessage(errorMessage);
                        })
                        .catch((error) => {
                            displayErrorMessage("An error occurred while calculating tax.");
                        });
                } else if (response.status === 500) {
                    response
                        .text()
                        .then((data) => {
                            displayErrorMessage(JSON.parse(data).details);
                        })
                        .catch((error) => {
                            displayErrorMessage("An error occurred while calculating tax.");
                        });
                } else {
                    throw new Error("An error occurred while calculating tax.");
                }
            })
            .catch((error) => {
                displayErrorMessage(error.message);
            });
    }

    function editTaxRecord(id) {
        var addTaxRecordContainer = document.getElementById("addTaxRecordContainer");
        addTaxRecordContainer.style.display = "none";

        const taxRecordRow = document.getElementById(`taxRecord_${id}`);
        const postalCodeCell = taxRecordRow.cells[0];
        const annualIncomeCell = taxRecordRow.cells[1];

        const editFormContainer = document.getElementById("editFormContainer");
        const editForm = document.getElementById("editForm");
        const editPostalCodeInput = document.getElementById("editPostalCode");
        const editAnnualIncomeInput = document.getElementById("editAnnualIncome");

        editPostalCodeInput.value = postalCodeCell.textContent;
        editAnnualIncomeInput.value = parseFloat(annualIncomeCell.textContent);

        editFormContainer.style.display = "block";

        editForm.addEventListener("submit", function (event) {
            event.preventDefault();

            const formData = new FormData(editForm);
            const updatedRecord = {
                postalCode: formData.get("editTaxCalculation.PostalCode"),
                annualIncome: parseFloat(formData.get("editTaxCalculation.AnnualIncome")),
            };

            updateTaxRecord(id, updatedRecord);
        });
    }

    function updateTaxRecord(id, updatedRecord) {
        fetch(`${_apiBaseUrl}/api/tax/records/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(updatedRecord),
        })
            .then((response) => {
                if (response.ok) {
                    location.reload();

                    cancelEdit();
                } else {
                    throw new Error("Failed to update tax record. Please try again.");
                }
            })
            .catch((error) => {
                displayErrorMessage(error.message);
            });
    }

    function cancelEdit() {
        var addTaxRecordContainer = document.getElementById("addTaxRecordContainer");
        addTaxRecordContainer.style.display = "block";

        const editFormContainer = document.getElementById("editFormContainer");
        const editForm = document.getElementById("editForm");

        editForm.reset();
        editFormContainer.style.display = "none";
    }

    function deleteTaxRecord(id) {
        const confirmDelete = confirm("Are you sure you want to delete this tax record?");
        if (!confirmDelete) {
            return;
        }

        fetch(`${_apiBaseUrl}/api/tax/records/${id}`, {
            method: "DELETE",
        })
            .then((response) => {
                if (response.ok) {
                    location.reload();
                } else {
                    throw new Error("Failed to delete tax record. Please try again.");
                }
            })
            .catch((error) => {
                displayErrorMessage(error.message);
            });
    }

    function clearFields(formName) {
        var form = document.forms[formName];

        if (form) {
            form.reset();
        }
    }

    function showNoResultsMessage() {
        const tableBody = document.querySelector("tbody");
        if (tableBody.childElementCount === 0) {
            const noResultsMessage = document.createElement("tr");
            noResultsMessage.innerHTML = `<td colspan="4" class="no-results-message">No results found.</td>`;
            tableBody.appendChild(noResultsMessage);
        }
    }

    window.addEventListener("DOMContentLoaded", showNoResultsMessage);

    function displayErrorMessage(message) {
        const errorMessageContainer = document.getElementById("errorMessageContainer");
        errorMessageContainer.innerHTML = `<div class="error-title-message"><small>Error</small></div><div>${message}</div>`;
        errorMessageContainer.style.display = "block";
        document.getElementById("errorContainer").style.display = "block";
    }

    function displayCatchError(error) {
        displayErrorMessage(error.message || "An error occurred.");
    }