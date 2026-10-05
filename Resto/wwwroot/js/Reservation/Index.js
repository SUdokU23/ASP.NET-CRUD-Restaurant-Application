function openDeleteModal(tableId) {
    console.log("Open delete modal with ID:", tableId);

    const tableIdDisplay = document.getElementById('tableIdDisplay');
    const tableIdInput = document.getElementById('tableIdInput');
    const modal = document.getElementById('_ConfirmDeleteTableModal');

    if (!tableIdDisplay || !tableIdInput || !modal) {
        console.error("One or more modal elements not found.");
        return;
    }

    tableIdDisplay.textContent = tableId;
    tableIdInput.value = tableId;
    modal.removeAttribute('inert');
    $('#_ConfirmDeleteTableModal').modal('show');
    modal.focus();
}

function closeModal() {
    const modal = document.getElementById('_ConfirmDeleteTableModal');
    if (modal) {
        $('#_ConfirmDeleteTableModal').modal('hide');
        modal.setAttribute('inert', 'inert');
    }
}

function openEditModal(tableId) {
    console.log("Open edit modal with ID:", tableId);

    const idInput = document.getElementById('idInput');
    const seats = document.getElementById('seats');
    const reserved = document.getElementById('reserved');
    const modal = document.getElementById('_EditModal');
    console.log("Open edit modal with ID:", idInput);
    console.log("Open edit modal with ID:", seats);
    console.log("Open edit modal with ID:", reserved);
    console.log("Open edit modal with ID:", modal);


    if (!idInput || !seats || !reserved || !modal) {
        console.error("One or more modal elements not found.");
        return;
    }

    
    idInput.value = tableId;
    modal.removeAttribute('inert');
    $('#_EditModal').modal('show');
    modal.focus();
}

function closeEditModal() {
    const modal = document.getElementById('_EditModal');
    if (modal) {
        $('#_EditModal').modal('hide');
        modal.setAttribute('inert', 'inert');
    }
}



    
