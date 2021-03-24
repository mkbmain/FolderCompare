using System.Collections.Generic;
using FolderCompare.Models.ResultStructure;

namespace FolderCompare.Models.ResultCalcModels
{
    public class Issues
    {
        public Issues()
        {
            DirectoryResultDetailsList = new List<DirectoryResultDetails>();
            FileResultDetailsList = new List<FileResultDetails>();
        }

        public List<DirectoryResultDetails> DirectoryResultDetailsList { get; set; }
        public List<FileResultDetails> FileResultDetailsList { get; set; }
    }
}