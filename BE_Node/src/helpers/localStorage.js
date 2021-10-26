module.exports = {
    getLocalStorage(name) {
        return JSON.parse(localStorage.getItem(name)) || [];
    },
    setLocalStorage(name, status) {
        localStorage.setItem(name, JSON.stringify(status));
    },
    deleteLocalStorage(name) {
        localStorage.removeItem(name);
    },
};