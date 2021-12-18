function getLocalStorage(name) {
    return JSON.parse(localStorage.getItem(name)) || [];
}

function setLocalStorage(name, status) {
    localStorage.setItem(name, JSON.stringify(status));
}

function deleteLocalStorage(name) {
    localStorage.removeItem(name);
}

if (document.querySelector('.message > .message-content').innerText !== '') {
    document.querySelector('.message').classList.add('show');
}

$(".alert").delay(3000).slideUp(200, function() {
    $(this).alert('close');
});