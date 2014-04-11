namespace FluentMvcGrid
{
    public class Configuration
    {
        private BootstrapVersion _bootstrapVersion;
        private WhiteSpace _whiteSpace;

        public Configuration()
        {
            _bootstrapVersion = BootstrapVersion.Bootstrap3;
            _whiteSpace = FluentMvcGrid.WhiteSpace.None;
        }

        public Configuration Bootstrap(BootstrapVersion value)
        {
            _bootstrapVersion = value;
            return this;
        }

        internal BootstrapVersion GetBootstrapVersion()
        {
            return _bootstrapVersion;
        }

        public Configuration WhiteSpace(WhiteSpace value)
        {
            _whiteSpace = value;
            return this;
        }

        internal WhiteSpace GetWhiteSpace()
        {
            return _whiteSpace;
        }
    }
}