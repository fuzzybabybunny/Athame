namespace Athame.PluginAPI.Service
{
    /// <summary>
    /// Represents a request for a new page of records.
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// The number of records per page.
        /// </summary>
        public int RecordsPerPage { get; set; }
        /// <summary>
        /// The identifier of the page to request.
        /// </summary>
        public string PageId { get; set; }
    }
}