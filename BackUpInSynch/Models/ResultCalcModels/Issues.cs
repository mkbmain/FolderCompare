using System.Collections.Generic;
using BackUpInSynch.Models.ResultStructure;

namespace BackUpInSynch.Models.ResultCalcModels
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