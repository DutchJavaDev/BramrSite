using System.Text;
using System.Collections.Generic;


namespace BramrSite.Models
{
    public class ApiResponse
    {
        public ApiResponse() { }

        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public object RequestedData { get; set; } = string.Empty;

        public ICollection<string> Errors { get; set; } = new List<string>();

        public static ApiResponse Oke(string message = "", object data = null)
        {
            return new ApiResponse { Success = true, Message = message, RequestedData = data };
        }

        public static ApiResponse Error(string message = "", object data = null, ICollection<string> errors = null)
        {
            return new ApiResponse { Success = false, Message = message, RequestedData = data, Errors = errors };
        }
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Sucess: {Success}");
            builder.AppendLine($"Message: {Message}");
            builder.AppendLine("\n[Errors]");

            if (Errors != null)
            {
                foreach (var error in Errors)
                {
                    builder.AppendLine($"{error}");
                }
            }

            return builder.ToString();
        }

    }
}
