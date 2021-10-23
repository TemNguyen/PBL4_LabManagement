const sidebarItems = document.querySelectorAll('.sidebar-item');
sidebarItems.forEach(sidebarItem => {
    sidebarItem.onclick = (e) => {
        e.preventDefault();
        const activeItem = document.querySelector('.sidebar-item.active');
        activeItem.classList.remove('active');
        sidebarItem.classList.add('active');
    }
});