using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;
using static BramrSite.Pages.Editor;

namespace BramrSite.Pages.Components
{
    public partial class ImageUpload : ComponentBase
    {
        [Inject] IFileReaderService FileReader { get; set; }
        [Inject] ApiService Api { get; set; }
        [Parameter] public ImageModel CurrentImage { get; set; }
        [Parameter] public Del CallBack { get; set; }
        ElementReference FileReference { get; set; }
        Stream FileStream { get; set; }
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
            using var memoryStream = await file.CreateMemoryStreamAsync((int)fileInfo.Size);
            FileStream = new MemoryStream(memoryStream.ToArray());
        }

        private async Task UploadImage()
        {
            IsDisabled = true;

            await Api.UploadImage(FileStream, CurrentImage.FileType.ToString());
            CurrentImage.FileUri = await Api.GetFileInfo(CurrentImage.FileType.ToString());
            CurrentImage.Src = $"https://localhost:44372/api/image/download/{CurrentImage.FileUri}";
            CallBackMethod(CallBack);
        }

        public void CallBackMethod(Del CallBackMethod)
        {
            CallBackMethod.Invoke(CurrentImage.FileUri, CurrentImage.Src);
        }
    }
}
