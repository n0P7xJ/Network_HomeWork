using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPoshtaProjectCore_1.Models
{
    public class WarehouseItemResponse
    {
        public string Ref { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionRu { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityRef { get; set; } = string.Empty;
        public string SettlementType { get; set; } = string.Empty;
    }

    public class WarehousePostModel
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string CalledMethod { get; set; } = string.Empty;
        public WarehouseMethodProperties? MethodProperties { get; set; }
    }

    public class WarehouseMethodProperties
    {

    }

    public class WarehouseResponse
    {
        public bool Success { get; set; }
        public List<WarehouseItemResponse>? Data { get; set; }
        public int? PageSize { get; set; }
        public List<object>? Errors { get; set; }
        public List<object>? Warnings { get; set; }
        public object? Info { get; set; }
        public List<object>? MessageCodes { get; set; }
        public List<object>? ErrorCodes { get; set; }
        public List<object>? WarningCodes { get; set; }
        public List<object>? InfoCodes { get; set; }
    }
}
