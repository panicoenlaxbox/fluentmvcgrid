using System;

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
        private bool _showIfEmpty = true;
        private int _totalCount;

        public FluentMvcGridPagination Enabled(bool enabled)
        {
            _enabled = enabled;
            return this;
        }

        internal bool GetEnabled()
        {
            return _enabled;
        }

        public FluentMvcGridPagination PageSize(int value)
        {
            _pageSize = value;
            return this;
        }

        public FluentMvcGridPagination TotalCount(int value)
        {
            _totalCount = value;
            return this;
        }

        public FluentMvcGridPagination PageIndex(int value)
        {
            _pageIndex = value;
            return this;
        }

        public FluentMvcGridPagination Sizing(PaginationSizing value)
        {
            _paginationSizing = value;
            return this;
        }

        [Obsolete("Unused")]
        public FluentMvcGridPagination ShowIfEmpty(bool value)
        {
            _showIfEmpty = value;
            return this;
        }

        public FluentMvcGridPagination NumericLinksCount(int value)
        {
            _numericLinksCount = value;
            return this;
        }

        public FluentMvcGridPagination Info(bool value)
        {
            _paginationInfo = value;
            return this;
        }

        public FluentMvcGridPagination HtmlAttributes(object value)
        {
            _htmlAttributes = value;
            return this;
        }

        internal string Build(Configuration configuration)
        {
            return Pagination.GetDefaultPagination(_pageIndex, _totalCount, _pageSize, _paginationSizing,
                    _numericLinksCount, _paginationInfo, _htmlAttributes, configuration.GetBootstrapVersion()).ToString();
        }
    }
}