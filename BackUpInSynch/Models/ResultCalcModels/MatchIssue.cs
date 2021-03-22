using System.Collections.Generic;

namespace BackUpInSynch.Models.ResultCalcModels
{
    internal class MatchIssue : Issues
    {
        public MatchIssue()
        {
            NamesMatched = new Dictionary<string, bool>();
        }

        public Dictionary<string, bool> NamesMatched { get; set; }
    }
}