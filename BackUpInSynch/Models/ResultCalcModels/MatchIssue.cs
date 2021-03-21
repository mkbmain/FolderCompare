using System.Collections.Generic;

namespace BackUpInSynch.Models.ResultCalcModels
{
    public class MatchIssue : Issues
    {
        public Dictionary<string, bool> NamesMatched { get; set; }
    }
    
}