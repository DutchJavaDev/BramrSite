using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;
using static BramrSite.Pages.CvEditor;
using static BramrSite.Pages.PortfolioEditor;

namespace BramrSite.Pages.Components
{
    public partial class ImageUpload : ComponentBase
    {
        [Inject] IFileReaderService FileReader { get; set; }
        [Inject] ApiService Api { get; set; }
        [Parameter] public ImageModel CurrentImage { get; set; }
        [Parameter] public CvImageDel CvCallBack { get; set; }
        [Parameter] public PortfolioImageDel PortfolioCallBack { get; set; }
        [Parameter] public bool IsCV { get; set; }
        ElementReference FileReference { get; set; }
        Stream FileStream { get; set; }
        private bool IsDisabled { get; set; } = true;
        private string ErrorMessage { get; set; }


        private async Task OpenImage()
        {
            var file = (await FileReader.CreateReference(FileReference).EnumerateFilesAsync()).FirstOrDefault();
            var fileInfo = await file.ReadFileInfoAsync();

            if (file == null)
            {
                ErrorMessage = "Geen bestand geselecteerd.";
                return;
            }

            if (fileInfo.Size > 1014572)
            {
                ErrorMessage = "Bestand mag niet groter zijn dan 10 mb.";
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
                ErrorMessage = "Alleen bestands types: png, jpg, jpeg toegestaan.";
            }
        }

        private async Task UploadImage()
        {
            IsDisabled = true;

            var apiResponse = await Api.UploadImage(FileStream, CurrentImage.Location.ToString(), IsCV);
            CurrentImage.FileUri = await Api.GetFileInfo(CurrentImage.Location.ToString());
#if DEBUG
            CurrentImage.Src = $"https://localhost:44372/api/image/download/{CurrentImage.FileUri}";
#else
            CurrentImage.Src = $"https://bramr.tech/api/image/download/{CurrentImage.FileUri}";
#endif
            if (IsCV)
            {
                CvCallBack.Invoke(CurrentImage.FileUri, CurrentImage.Src);
            }
            else
            {
                PortfolioCallBack.Invoke(CurrentImage.FileUri, CurrentImage.Src);
            }
            
            if (apiResponse.Success == true)
            {
                ErrorMessage = "Succesvol geüpload.";
            }
            else
            {
                ErrorMessage = apiResponse.Message;
            }
        }
    }
}
