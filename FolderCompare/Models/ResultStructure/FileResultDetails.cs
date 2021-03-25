using FolderCompare.Models.ScanStructure;

namespace FolderCompare.Models.ResultStructure
{
    public class FileResultDetails : ResultDetailsBase<FileNode>
    {
        public FileNode Linked { get; set; }
    }
}