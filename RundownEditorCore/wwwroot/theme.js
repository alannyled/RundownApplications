window.themeFunctions = {
    toggleTheme: function () {
        var currentTheme = document.documentElement.getAttribute("data-bs-theme");
        var newTheme = currentTheme === "dark" ? "light" : "dark";
        document.documentElement.setAttribute("data-bs-theme", newTheme);
        localStorage.setItem("theme", newTheme);
        document.cookie = `theme=${newTheme}; path=/; max-age=31536000`; //  1 år
    },
    isDarkMode: function () {
        return document.documentElement.getAttribute("data-bs-theme") === "dark";
    },
    // anvender temaet fra cookie eller localStorage
    // fordi det er en SPA vil siden blinke, hvis der ikke er en cookie
    // fordi det er en SPA vil et tema valg ikke holde på tværs af sider, hvis ikke det også er i localStorage, da headeren kun læses første gang
    // derfor gemmes temaet i begge steder
    setTheme: function (theme) {
        document.documentElement.setAttribute("data-bs-theme", theme);
        localStorage.setItem("theme", theme);
        document.cookie = `theme=${theme}; path=/; max-age=31536000`; // Gem temaet i cookie
    },
    getTheme: function () {
        const match = document.cookie.match(new RegExp('(^| )theme=([^;]+)'));
        return match ? match[2] : localStorage.getItem('theme') || 'dark'; // Fallback til localStorage eller "dark"
    }
};