using BramrSite.Classes;
using Microsoft.AspNetCore.Components;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;

namespace BramrSite.Pages.Components
{
    public partial class ImageUpload : ComponentBase
    {
        [Inject] IFileReaderService FileReader { get; set; }
        [Inject] ApiService Api { get; set; }
        [Parameter] public string ImageSrc { get; set; }

        ElementReference FileReference { get; set; }
        Stream FileStream { get; set; }
        string FileName { get; set; }
        private bool IsDisabled { get; set; } = true;

        private async Task OpenImage()
        {
            var file = (await FileReader.CreateReference(FileReference).EnumerateFilesAsync()).FirstOrDefault();

            if(file == null)
            {
                return;
            }
            IsDisabled = false;

            var fileInfo = await file.ReadFileInfoAsync();
            FileName = fileInfo.Name;
            using var memoryStream = await file.CreateMemoryStreamAsync((int)fileInfo.Size);
            FileStream = new MemoryStream(memoryStream.ToArray());
        }

        private async Task UploadImage()
        {
            IsDisabled = true;

            await Api.UploadImage(FileStream, FileName);

            ImageSrc = "https://localhost:44372/api/image/download";
        }
    }
}
