using System;

namespace BackUpInSynch.Models.ScanStructure
{
    public abstract class NodeBase
    {
        public Guid Id = Guid.NewGuid();
        public string FullLocation { get; set; }
        public string BasePath { get; set; }
        public string RelativeLocation => FullLocation.StartsWith(BasePath) ?  FullLocation.Substring(BasePath.Length) : throw new Exception("Path miss match");
        
        public string Name { get; set; }
    }
}