using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class DirectorThumbnail
    {
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public byte[]? Content { get; set; }

        [StringLength(255)]
        public string? MimeType { get; set; }

        public int DirectorID { get; set; }
        public Director? Director { get; set; }
    }
}
