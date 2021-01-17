using static BramrSite.Models.EditMode;

namespace BramrSite.Models
{
    public enum EditMode
    {
        Done,
        Edit
    }

    public class NoteModel
    {
        public string Text { get; set; }

        public EditMode EditMode { get; set; }
    }
}
