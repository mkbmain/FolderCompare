using System.Collections.Generic;
using System.Linq;
using FolderCompare.Models.ResultCalcModels;
using FolderCompare.Models.ResultStructure;
using FolderCompare.Models.ScanStructure;
using FolderCompare.Utils;

namespace FolderCompare.CalculateMissMatches
{
    internal static class CalculateDifferencesDirectories
    {
        public static ResultPotentialIssues Issues(string sourceBasePath, string destinationBasePath, DirectoryNode source,
            DirectoryNode dest, bool checkContents)
        {
            var output = new ResultPotentialIssues
            {
                DirectoryResultDetailsList = new List<DirectoryResultDetails>(),
                FileResultDetailsList = new List<FileResultDetails>()
            };

            if ((dest == null && source == null) || dest == source)
            {
                return output;
            }

            if (dest == null)
            {
                output.DirectoryResultDetailsList.Add(
                    ResultActionGenerator.FolderGenerator(source, destinationBasePath));
                return output;
            }

            if (source == null)
            {
                var result = ResultActionGenerator.FolderGenerator(dest, sourceBasePath);
                // we flip them as this is the destination the bottom option is recommended
                result.ActionHandlerWithTexts = result.ActionHandlerWithTexts.Reverse();
                output.DirectoryResultDetailsList.Add(result);
                return output;
            }

            var items = SortDirectories(sourceBasePath, destinationBasePath, source, dest, checkContents);
            output.DirectoryResultDetailsList.AddRange(items.DirectoryResultDetailsList);
            output.FileResultDetailsList.AddRange(items.FileResultDetailsList);

            // i know this looks like a duplicate but the names being parsed in on last arg switches logic inside method to do dest not source 
            items = SortDirectories(sourceBasePath, destinationBasePath, source, dest, checkContents, items.NamesMatched);
            output.DirectoryResultDetailsList.AddRange(items.DirectoryResultDetailsList);
            output.FileResultDetailsList.AddRange(items.FileResultDetailsList);

            output.FileResultDetailsList.AddRange(CalculateDifferencesFiles.ManageFiles(sourceBasePath, destinationBasePath, source, dest, checkContents));
            return output;
        }
        

        private static MatchResultPotentialIssue SortDirectories(string sourceBasePath, string destinationBasePath,
            DirectoryNode source, DirectoryNode dest, bool checkContents, IReadOnlyDictionary<string, bool> namesDone = null)    // names done switches source and dest with in method
        {
            var output = new MatchResultPotentialIssue();
            // this is very specific logic that tightly bounds it to above
            var gothrough = namesDone == null
                ? source.SubDirectories
                : dest.SubDirectories.Where(f => namesDone.ContainsKey(f.Name) == false);
            foreach (var item in gothrough)
            {
                var match = namesDone == null ? dest : source;
                var subMatch = match.SubDirectories.FirstOrDefault(f => f.Name == item.Name);
                output.NamesMatched.Add(item.Name, true);
                var complete = Issues(match == dest ? sourceBasePath : destinationBasePath,
                    match == dest ? destinationBasePath : sourceBasePath, item, subMatch, checkContents);

                foreach (var dir in complete.DirectoryResultDetailsList)
                {
                    if (namesDone != null)
                    {
                        dir.Source = false;
                    }

                    output.DirectoryResultDetailsList.Add(dir);
                }

                foreach (var file in complete.FileResultDetailsList)
                {
                    if (namesDone != null)
                    {
                        file.Source = false;
                    }

                    output.FileResultDetailsList.Add(file);
                }
            }

            return output;
        }
        
    }
}