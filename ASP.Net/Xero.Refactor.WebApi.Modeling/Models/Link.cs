namespace Xero.Refactor.WebApi.Modeling
{

    /// <summary>
    /// Link for HATEOAS
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Can be self of the name of the Navigation object
        /// </summary>
        public string Rel { get; set; }
        /// <summary>
        /// The link
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// Which verb GET, POST, PUT, DELETE, PATCH should be used
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// The type of the object. Can be
        /// - text/xml","application/json, for Get
        /// - application/x-www-form-urlencoded, for POST, PUT, PATCH
        /// </summary>
        public string Type { get; set; }
    }
}
