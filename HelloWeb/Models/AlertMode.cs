using HelloWeb.Enumerations;

namespace HelloWeb.Models
{
	public class AlertMode
	{
		public AlertType Type { get; set; }
		public string Name { get; set; }
		public bool IsSelected { get; set; }
		public string ChangeTypeUrl { get; set; }
	}
}
