function openDeleteModal(itemId) {
    console.log("Open delete modal with ID:", itemId);

    const itemIdDisplay = document.getElementById('itemIdDisplay');
    const itemIdInput = document.getElementById('itemIdInput');
    const modal = document.getElementById('_ConfirmDeleteItemModal');

    if (!itemIdDisplay || !itemIdInput || !modal) {
        console.error("One or more modal elements not found.");
        return;
    }

    itemIdDisplay.textContent = itemId;
    itemIdInput.value = itemId;
    modal.removeAttribute('inert');
    $('#_ConfirmDeleteItemModal').modal('show');
    modal.focus();
}

function closeModal() {
    const modal = document.getElementById('_ConfirmDeleteItemModal');
    if (modal) {
        $('#_ConfirmDeleteItemModal').modal('hide');
        modal.setAttribute('inert', 'inert');
    }
}
function openAddCategoryModal() {
    console.log("Open category modal");

    const modal = document.getElementById('_AddCategory');
    
    
    if (!modal) {
        console.error("One or more modal elements not found.");
        return;
    }
    modal.removeAttribute('inert');
    $('#_AddCategory').modal('show');
    modal.focus();
}

function openDeleteCategoryModal() {
    modal = document.getElementById('_DeleteCategory');
    const categorySelect = document.getElementById('categorySelect');
    categorySelect.innerHTML = '';  // Clear existing options

    categories.forEach(category => {
        const option = document.createElement('option');
        option.value = category; 
        option.textContent = category;
        categorySelect.appendChild(option);
    });

    modal.removeAttribute('inert');
    $('#_DeleteCategory').modal('show'); 
    modal.focus();
}

function closeDeleteCategoryModal() {
    const modal = document.getElementById('_DeleteCategory');
    if (modal) {
        $('#_DeleteCategory').modal('hide');
        modal.setAttribute('inert', 'inert');
    }
}