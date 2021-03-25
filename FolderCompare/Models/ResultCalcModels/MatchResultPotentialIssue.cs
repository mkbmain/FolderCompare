using System.Collections.Generic;

namespace FolderCompare.Models.ResultCalcModels
{
    internal class MatchResultPotentialIssue : ResultPotentialIssues
    {
        public MatchResultPotentialIssue()
        {
            NamesMatched = new Dictionary<string, bool>();
        }

        public Dictionary<string, bool> NamesMatched { get; set; }
    }
}