using System;

namespace BackUpInSynch.Models.ScanStructure
{
    public abstract class NodeBase
    {
        public Guid Id = Guid.NewGuid();
        public string FullLocation { get; set; }
        public string BasePath { get; set; }
        public string RelativeLocation => FullLocation.Replace(BasePath, "");
        
        public string Name { get; set; }
    }
}