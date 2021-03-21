using System.Collections.Generic;
using BackUpInSynch.Models.ResultStructure;

namespace BackUpInSynch.Models.ResultCalcModels
{
    public class Issues
    {
        public List<DirectoryResultDetails> DirectoryResultDetailsList { get; set; }
        public List<FileResultDetails> FileResultDetailsList { get; set; }
    }
}