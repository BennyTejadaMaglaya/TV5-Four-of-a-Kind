using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
	public class HomeImage
	{
		public int ID { get; set; }

		[ScaffoldColumn(false)]
		public byte[]? Content { get; set; }

		[StringLength(255)]
		public string? MimeType { get; set; }

		[StringLength(200, ErrorMessage = "Maximum 200 characters.")]
		[Display(Name = "Welcome Message")]
		public string? WelcomeMessage { get; set; }

		[StringLength(60, ErrorMessage = "Maximum 60 characters.")]
		[Display(Name = "Button Text")]
		public string? ButtonText { get; set; }
	}
}
