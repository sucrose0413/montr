using Montr.Metadata.Models;

namespace Tendr.Models
{
	public class EventSearchRequest : Paging
    {
        public string Name { get; set; }
    }
}