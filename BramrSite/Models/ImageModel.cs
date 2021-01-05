namespace BramrSite.Models
{
    public class ImageModel
    {
        public int Location { get; set; }

        public int Width { get; set; } = 100;
        public int Height { get; set; } = 1000;
        public string Src { get; set; }
        public string Alt { get; set; }
        public int Border { get; set; } 
        public Float FloatSet { get; set; }
        public double Opacity { get; set; } = 1;
        public ObjectFit ObjectFitSet{ get; set; } 
        //public int Margin { get; set; }
        public int Padding { get; set; }

        public string FileUri { get; set; }
        public FileTypes FileType { get; set; }

        public bool Selected { get; set; }

        public enum Float
        {
            none,
            left,
            right

        }
        public enum ObjectFit
        {
            cover,                      
            fill,
            contain,
            none
        }

        public enum FileTypes
        {
            ProfielFoto
        }

    }
}
