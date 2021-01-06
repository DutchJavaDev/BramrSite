namespace BramrSite.Models
{
    public class TextModel
    {
        public int Location { get; set; }
        public string Text { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underlined { get; set; }
        public Allignment TextAllignment { get; set; }
        public bool StrikedThrough { get; set; }
        public string TemplateType { get; set; }
        public int FontSize { get; set; } = 10;

        public bool Selected { get; set; }

        public enum Allignment
        {
            Left,
            Center,
            Right
        }
    }
}