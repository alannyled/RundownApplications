using Microsoft.JSInterop;

namespace RundownEditorCore.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime _jsRuntime;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Kalder JS, som anvender både localstorage og cookies til at sætte temaet
        /// Fordi det er en SPA, er det vigtigt at temaet huskes på tværs af sessioner, da det ellers vil skifte tilbage til standard temaet ved hver sideindlæsning
        /// Cookies bruges til at gemme temaet, så det huskes næste gang og forhindrer blink af forkert tema ved første indlæsning
        /// Local storage bruges til at gemme temaet, så det huskes på tværs af sessioner, mens siden er i brug
        /// </summary>
        /// <returns></returns>
        public async Task SetTheme()
        {
            var theme = await _jsRuntime.InvokeAsync<string>("themeFunctions.getTheme");
            await _jsRuntime.InvokeVoidAsync("themeFunctions.setTheme", theme);
        }

        public async Task SetTheme(string theme)
        {
            await _jsRuntime.InvokeVoidAsync("themeFunctions.setTheme", theme);
        }
    }
}
