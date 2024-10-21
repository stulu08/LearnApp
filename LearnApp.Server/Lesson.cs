using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnApp.Server
{
	public class ImageUploadResult
	{
		public string? path { get; set; }
	}

	public enum Subject {
		German, English, Math, 
		Chemistry, Biology, Physics,
		ComputerScience, History, Politics
	}
	public class Lesson {
		[Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id {  get; set; }
		[Required]
		public int user {  get; set; }
		[Required, Length(3, 32)]
		public string? titel {  get; set; }
		[Required]
		public Subject lesson {  get; set; }
		[Required]
		public string? description {  get; set; }
		[Required]
		public ICollection<string>? tags { get; set; }
		[Required]
		public int duration { get; set; }
		[Required]
		public int price { get; set; }
	}
}
