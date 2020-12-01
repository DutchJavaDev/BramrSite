using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BramrSite.Models
{
    public class ApiResponseModel
    {
        private ApiResponseModel() { }

        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public object RequestedData { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Success} {Message} {RequestedData}";
        }
    }
}
