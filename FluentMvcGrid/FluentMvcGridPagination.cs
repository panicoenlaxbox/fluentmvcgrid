using System;

namespace FluentMvcGrid
{
    public class FluentMvcGridPagination
    {
        private object _attributes;
        private bool _enabled;
        private int _numericLinksCount;
        private int _pageIndex;
        private int _pageSize;
        private bool _paginationInfo;
        private PaginationSizing _paginationSizing;
        private PaginationAligment _paginationAligment;
        private int _totalCount;
        private string _onPaginate;

        public FluentMvcGridPagination()
        {
            _enabled = true;
            _numericLinksCount = 5;
            _pageIndex = 1;
            _pageSize = 10;
            _paginationInfo = true;
            _paginationSizing = PaginationSizing.Normal;
            _paginationAligment = PaginationAligment.Left;
        }

        public FluentMvcGridPagination Attributes(object value)
        {
            _attributes = value;
            return this;
        }

        public FluentMvcGridPagination Enabled(bool value)
        {
            _enabled = value;
            return this;
        }

        public FluentMvcGridPagination Info(bool value)
        {
            _paginationInfo = value;
            return this;
        }

        public FluentMvcGridPagination NumericLinksCount(int value)
        {
            _numericLinksCount = value;
            return this;
        }

        public FluentMvcGridPagination PageIndex(int value)
        {
            _pageIndex = value;
            return this;
        }

        public FluentMvcGridPagination PageSize(int value)
        {
            _pageSize = value;
            return this;
        }

        public FluentMvcGridPagination Sizing(PaginationSizing value)
        {
            _paginationSizing = value;
            return this;
        }

        public FluentMvcGridPagination OnPaginate(string value)
        {
            _onPaginate = value;
            return this;
        }

        public FluentMvcGridPagination Aligment(PaginationAligment value)
        {
            _paginationAligment = value;
            return this;
        }

        public FluentMvcGridPagination TotalCount(int value)
        {
            _totalCount = value;
            return this;
        }

        internal string Build(Configuration configuration, Uri url)
        {
            return Pagination.GetDefaultPagination(_pageIndex, _totalCount, _pageSize, _paginationSizing, _paginationAligment,
                    _numericLinksCount, _paginationInfo, _attributes, configuration.GetBootstrapVersion(), _onPaginate, url).ToString();
        }

        internal bool GetEnabled()
        {
            return _enabled;
        }
    }
}