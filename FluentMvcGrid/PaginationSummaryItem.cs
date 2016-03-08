namespace FluentMvcGrid
{
    public class PaginationSummaryItem
    {
        private readonly bool _enabled;
        private readonly int? _pageIndex;

        public bool Enabled
        {
            get { return _enabled; }
        }

        public int? PageIndex
        {
            get { return _pageIndex; }
        }

        public PaginationSummaryItem(bool enabled, int? pageIndex)
        {
            _enabled = enabled;
            _pageIndex = pageIndex;
        }
    }
}