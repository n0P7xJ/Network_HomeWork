using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPoshtaProjectCore_1.Models
{
    [Table("tbl_Area")]
    public class Area
    {
        [Key]
        public string Ref { get; set; } // Унікальний ідентифікатор області
        public string AreaName { get; set; } // Назва області
        public string AreaDescription { get; set; } // Опис українською
    }

    [Table("tbl_City")]
    public class City
    {
        [Key]
        public string Ref { get; set; } // Унікальний ідентифікатор міста
        public string Description { get; set; } // Назва міста українською
        public string AreaRef { get; set; } // Зв'язок із областю

        [ForeignKey("AreaRef")]
        public Area Area { get; set; }
    }

    [Table("tbl_Warehouse")]
    public class Warehouse
    {
        [Key]
        public string Ref { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string CityRef { get; set; }
        public string SettlementType { get; set; }

        [ForeignKey("CityRef")]
        public City City { get; set; }
    }
}
