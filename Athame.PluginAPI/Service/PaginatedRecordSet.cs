using System;
using System.Collections.Generic;

namespace Athame.PluginAPI.Service
{
    /// <summary>
    /// Represents a page of records.
    /// </summary>
    /// <typeparam name="T">The type of the records returned.</typeparam>
    public class PaginatedRecordSet<T>
    {
        private readonly PageRequest request;
        public PaginatedRecordSet(PageRequest request, string previousPageId, string nextPageId)
        {
            PageId = request.PageId;
            PreviousRequest = String.IsNullOrEmpty(previousPageId)
                ? null
                : new PageRequest {PageId = previousPageId, RecordsPerPage = request.RecordsPerPage};
            NextRequest = String.IsNullOrEmpty(nextPageId)
                ? null
                : new PageRequest {PageId = nextPageId, RecordsPerPage = request.RecordsPerPage};
        }

        /// <summary>
        /// The identifier of the current page.
        /// </summary>
        public string PageId { get; private set; }
        /// <summary>
        /// The identifier of the previous page, or null if this is the first page.
        /// </summary>
        public PageRequest PreviousRequest { get; private set; }
        /// <summary>
        /// The identifier of the next page, or null if this is the last page.
        /// </summary>
        public PageRequest NextRequest { get; private set; }
        /// <summary>
        /// The total number of records available.
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// The records in this current page.
        /// </summary>
        public IEnumerable<T> Records { get; set; }

    }
}