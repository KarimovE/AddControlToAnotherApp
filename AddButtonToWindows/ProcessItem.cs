namespace AddButtonToWindows
{
    public class ProcessItem
    {
        public string ProcessName { get; set; }
        public string Title { get; set; }

        public ProcessItem(string processname, string title)
        {
            this.ProcessName = processname;
            this.Title = title;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Title))
                return string.Format("{0} ({1})", this.ProcessName, this.Title);
            else
                return string.Format("{0}", this.ProcessName);
        }
    }
}
