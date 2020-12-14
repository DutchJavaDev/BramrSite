using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using System;
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
        private string ErrorMessage { get; set; }
        private string FileName { get; set; }

        private async Task OpenImage()
        {
            var file = (await FileReader.CreateReference(FileReference).EnumerateFilesAsync()).FirstOrDefault();
            var fileInfo = await file.ReadFileInfoAsync();
            
            var foo = fileInfo.Name.Length;
            FileName = fileInfo.Name.Substring(0, 10);
            Console.WriteLine(FileName);
            if (file == null)
            {
                
            }

            Console.WriteLine(fileInfo.Size);
            if (fileInfo.Size > 314572)
            {
                ErrorMessage = "Bestand mag niet groter zijn dan 3 mb.";
                return;
            }

            if (fileInfo.Type == "image/png" || fileInfo.Type == "image/jpeg" || fileInfo.Type == "image/jpg")
            {
                using var memoryStream = await file.CreateMemoryStreamAsync((int)fileInfo.Size);
                FileStream = new MemoryStream(memoryStream.ToArray());
                IsDisabled = false;
                return;
            }

            else
            {
              ErrorMessage = "Only accept files fo type: png, jpg, jpeg";
            }
        }

        private async Task UploadImage()
        {
            IsDisabled = true;

            var apiResponse = await Api.UploadImage(FileStream, CurrentImage.FileType.ToString());
            CurrentImage.FileUri = await Api.GetFileInfo(CurrentImage.FileType.ToString());
            CurrentImage.Src = $"https://localhost:44372/api/image/download/{CurrentImage.FileUri}";
            CallBackMethod(CallBack);
            if(apiResponse.Success == true)
            {
                ErrorMessage = "Succesfully uploaded";
            }
            else
            {
                ErrorMessage = apiResponse.Message;
            }
        }

        public void CallBackMethod(Del CallBackMethod)
        {
            CallBackMethod.Invoke(CurrentImage.FileUri, CurrentImage.Src);
        }
    }
}
