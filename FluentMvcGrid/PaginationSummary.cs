namespace FluentMvcGrid
{
    public class PaginationSummary
    {
        private readonly int _pageCount;
        private readonly int _pageIndex;
        private readonly int _totalCount;
        private readonly int _pageSize;
        private readonly PaginationSummaryItem _first;
        private readonly PaginationSummaryItem _previous;
        private readonly PaginationSummaryItem _next;
        private readonly PaginationSummaryItem _last;

        public PaginationSummaryItem First
        {
            get { return _first; }
        }

        public PaginationSummaryItem Previous
        {
            get { return _previous; }
        }

        public PaginationSummaryItem Next
        {
            get { return _next; }
        }

        public PaginationSummaryItem Last
        {
            get { return _last; }
        }

        public int PageCount
        {
            get { return _pageCount; }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
        }

        public int TotalCount
        {
            get { return _totalCount; }
        }

        public int PageSize
        {
            get { return _pageSize; }
        }

        public PaginationSummary(int pageCount, int pageIndex, int totalCount, int pageSize, PaginationSummaryItem first, PaginationSummaryItem previous, PaginationSummaryItem next, PaginationSummaryItem last)
        {
            _pageCount = pageCount;
            _pageIndex = pageIndex;
            _totalCount = totalCount;
            _pageSize = pageSize;
            _first = first;
            _previous = previous;
            _next = next;
            _last = last;
        }

        public override string ToString()
        {
            return string.Format("First: {0} {1}" +
                   ", Previous: {2} {3}" +
                   ", Next: {4} {5}" +
                   ", Last: {6} {7}", First.Enabled, First.PageIndex, Previous.Enabled, Previous.PageIndex, Next.Enabled, Next.PageIndex, Last.Enabled, Last.PageIndex);
        }
    }
}