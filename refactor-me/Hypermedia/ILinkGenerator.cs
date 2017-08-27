using System;
using System.Web.Http.Routing;
using Xero.Refactor.WebApi.Modeling;

namespace Xero.Refactor.WebApi.Hypermedia
{
    public interface ILinkGenerator
    {
        void PopulateLinksOnBasicVerbs(ILinkResource linkResource,
                                    UrlHelper urlHelper,
                                    string controlerName,
                                    Guid id);
    }

    public class LinkGenerator : ILinkGenerator
    {
        public void PopulateLinksOnBasicVerbs(ILinkResource linkResource, UrlHelper urlHelper, string controlerName, Guid id)
        {
            if (linkResource == null || urlHelper == null)
                return;

            linkResource.Links.Add(
             new Link()
             {
                 Rel = "self",
                 Action = "GET",
                 Href = urlHelper.Route("DefaultApi", new { controller = controlerName, id = Guid.NewGuid() }),
                 Type = "\"text/xml\",\"application/json\""
             });
            linkResource.Links.Add(
             new Link()
             {
                 Rel = "self",
                 Action = "PUT",
                 Href = urlHelper.Route("DefaultApi", new { controller = controlerName, id = Guid.NewGuid() }),
                 Type = "application/x-www-form-urlencoded"
             });

            linkResource.Links.Add(
            new Link()
            {
                Rel = "self",
                Action = "DELETE",
                Href = urlHelper.Route("DefaultApi", new { controller = controlerName, id = Guid.NewGuid() }),
                Type = ""
            });

            linkResource.Links.Add(
            new Link()
            {
                Rel = "self",
                Action = "GET",
                Href = urlHelper.Route("DefaultApi", new { controller = controlerName, id = Guid.NewGuid() }),
                Type = "\"text/xml\",\"application/json\""
            });
        }
    }

}