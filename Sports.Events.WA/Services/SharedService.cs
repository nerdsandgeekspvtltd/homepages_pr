using Microsoft.JSInterop;
using System.Text;

namespace Sports.Events.WA.Services
{
    public class SharedService
    {
        private int _fontSize = 14;
        private int _fontSizeHeading = 20;
        public event Action OnChange;
        public bool IsDarkMode { get; private set; }

        public int FontSize
        {
            get => _fontSize;
            private set
            {
                _fontSize = Math.Clamp(value, 10, 24);
                NotifyStateChanged();
            }
        }

        public int FontSizeHead
        {
            get => _fontSizeHeading;
            private set
            {
                _fontSizeHeading = Math.Clamp(value, 10, 24);
                NotifyStateChanged();
            }
        }

        public void AdjustFontSize(int change)
        {
            FontSize += change;
            FontSizeHead += change;
        }

        public void ResetFontSize()
        {
            FontSize = 16;
            FontSizeHead = 20;
        }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
