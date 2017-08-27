using System.Collections.Generic;

namespace Xero.Refactor.WebApi.Modeling
{
    public interface ILinkResource
    {
        List<Link> Links { get; set; }
    }

    public class LinkResource : ILinkResource
    {
        public List<Link> Links { get; set; }
    }
}
