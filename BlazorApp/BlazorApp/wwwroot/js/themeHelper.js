
window.toggleTheme = (theme) => {
    if (theme === "dark") {
        document.documentElement.classList.add("dark");
    } else {
        document.documentElement.classList.remove("dark");
    }
};

window.initializeTheme = (isDark) => {
    if (isDark) {
        document.documentElement.classList.add("dark");
    } else {
        document.documentElement.classList.remove("dark");
    }
};

window.applySavedTheme = function () {
    var themePreference = localStorage.getItem('themePreference');
    if (themePreference === 'true') {
        document.documentElement.classList.add('dark');
    } else {
        document.documentElement.classList.remove('dark');
    }
};

window.applyThemeOnNavigation = function () {
    console.log("Navigation event listener added");
    setInterval(function () {
        if (window.location.href !== sessionStorage.getItem('previousUrl')) {
            console.log("Navigation event occurred");
            applySavedTheme();
            sessionStorage.setItem('previousUrl', window.location.href);
        }
    }, 400); 
};
document.addEventListener('DOMContentLoaded', function () {
    window.applySavedTheme();
    applyThemeOnNavigation();
});

