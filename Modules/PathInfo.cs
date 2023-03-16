#nullable disable

namespace RestartAutomation.Modules
{
    public class PathInfo
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public List<ModuleInfo> ModuleInfos { get; set; }
    }

    public class ModuleInfo
    {
        public string Name{get; set;}
        public List<string> Areas{get; set;}
        public int Option {get; set;}

    }
}