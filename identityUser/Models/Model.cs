namespace identityUser.Models
{
	public class Model
	{
		public int ID { get; set; }
		public string Name { get; set; } = "";
		public int MakeID { get; set; }

		public Make Make { get; set; }
	}
}
