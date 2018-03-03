using System.Collections.Generic;

namespace petapoco
{
    public class AppSettings {
        public List<Entry> ConnectionStrings { get; set; }
    }

    public class Entry {
        public string ProviderName { get; set; }
        public string ConnectionString { get; set; }
    }
}