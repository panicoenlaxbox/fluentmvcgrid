namespace FluentMvcGrid
{
    public class PaginationShortcut
    {
        public PaginationShortcutItem First { get; set; }
        public PaginationShortcutItem Previous { get; set; }
        public PaginationShortcutItem Next { get; set; }
        public PaginationShortcutItem Last { get; set; }

        public override string ToString()
        {
            return string.Format("First: {0} {1}" +
                   ", Previous: {2} {3}" +
                   ", Next: {4} {5}" +
                   ", Last: {6} {7}", First.Enabled, First.PageIndex, Previous.Enabled, Previous.PageIndex, Next.Enabled, Next.PageIndex, Last.Enabled, Last.PageIndex);
        }
    }
}