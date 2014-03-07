namespace FluentMvcGrid
{
    public class FluentMvcGridPagination
    {
        private bool _enabled = true;
        private object _htmlAttributes;
        private int _numericLinksCount = 5;
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private bool _paginationInfo = true;
        private PaginationSizing _paginationSizing = PaginationSizing.Normal;
        private int _totalCount;

        public FluentMvcGridPagination Enabled(bool enabled)
        {
            _enabled = enabled;
            return this;
        }

        internal bool IsEnabled()
        {
            return _enabled;
        }

        public FluentMvcGridPagination PageSize(int pageSize)
        {
            _pageSize = pageSize;
            return this;
        }

        public FluentMvcGridPagination TotalCount(int totalCount)
        {
            _totalCount = totalCount;
            return this;
        }

        public FluentMvcGridPagination PageIndex(int pageIndex)
        {
            _pageIndex = pageIndex;
            return this;
        }

        public FluentMvcGridPagination Sizing(PaginationSizing paginationSizing)
        {
            _paginationSizing = paginationSizing;
            return this;
        }

        public FluentMvcGridPagination NumericLinksCount(int numericLinksCount)
        {
            _numericLinksCount = numericLinksCount;
            return this;
        }

        public FluentMvcGridPagination Info(bool paginationInfo)
        {
            _paginationInfo = paginationInfo;
            return this;
        }

        public FluentMvcGridPagination HtmlAttributes(object htmlAttributes)
        {
            _htmlAttributes = htmlAttributes;
            return this;
        }

        internal string Build()
        {
            return Pager.GetDefaultPagination(_pageIndex, _totalCount, _pageSize, _paginationSizing,
                    _numericLinksCount, _paginationInfo, _htmlAttributes).ToString();
        }
    }
}