const sidebarItems = document.querySelectorAll('.sidebar-item');
sidebarItems.forEach(sidebarItem => {
    sidebarItem.onclick = (e) => {
        e.preventDefault();
        const activeItem = document.querySelector('.sidebar-item.active');
        activeItem.classList.remove('active');
        sidebarItem.classList.add('active');
    }
});

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
