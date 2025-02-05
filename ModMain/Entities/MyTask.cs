using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_TMS.ModMain.Entities
{
	[Table("task")]
	public record MyTask
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public int Id { get; set; }
		[Column("title")]
		[MaxLength(100)]
		public string Title { get; set; } = string.Empty;
		[Column("description")]
		public string Description { get; set; } = string.Empty;
		[Required]
		[Column("status")]
		public string Status { get; set; } = string.Empty; // Possible values: "Pending", "In Progress", "Completed"
		[Column("createdat")]
		public DateTime CreatedAt { get; set; }
	}

}
