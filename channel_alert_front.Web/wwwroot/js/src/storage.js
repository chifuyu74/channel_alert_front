const storage = {
    local: {
        getStorage(key) {
            return localStorage.getItem(key);
        },
        setStorage(key, value) {
            localStorage.setItem(key, value);
        },
        removeStorage(key) {
            localStorage.removeItem(key);
        }
    },
    session: {
        getStorage(key) {
            return sessionStorage.getItem(key);
        },
        setStorage(key, value) {
            sessionStorage.setItem(key, value);
        },
        removeStorage(key) {
            sessionStorage.removeItem(key);
        }
    }
};

window.storage = storage;