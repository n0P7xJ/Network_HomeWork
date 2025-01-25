using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPoshtaProjectCore_1.Models
{
    public class AreaItemResponse
    {
        public string Ref { get; set; } = string.Empty;
        public string AreasCenter { get; set; } = string.Empty;
        public string DescriptionRu { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    public class AreaPostModel
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string CalledMethod { get; set; } = string.Empty;

        public MethodProperties? MethodProperties { get; set; }
    }
    public class AreaResponse
    {
        public bool Success { get; set; }
        public List<AreaItemResponse>? Data { get; set; }
        public List<object>? Errors { get; set; }
        public List<object>? Warnings { get; set; }
        public List<object>? Info { get; set; }
        public List<object>? MessageCodes { get; set; }
        public List<object>? ErrorCodes { get; set; }
        public List<object>? WarningCodes { get; set; }
        public List<object>? InfoCodes { get; set; }
    }
    public class MethodProperties
    {
        // Можна додавати інші властивості, якщо вони будуть потрібні в майбутньому
    }
}
