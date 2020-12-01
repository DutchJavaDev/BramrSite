using System.Text;
using System.Collections.Generic;


namespace BramrSite.Models
{
    public class ApiResponseModel
    {
        private ApiResponseModel() { }

        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public ICollection<string> Errors { get; private set; } = new List<string>();

        public object RequestedData { get; set; } = string.Empty;

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Sucess: {Success}");
            builder.AppendLine($"Message: {Message}");
            builder.AppendLine("\n[Errors]");

            foreach (var error in Errors)
            {
                builder.AppendLine($"{error}");
            }

            return $"{Success} {Message} {RequestedData}";
        }
    }
}
