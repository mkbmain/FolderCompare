using System.Collections.Generic;
using System.Linq;
using BackUpInSynch.Models.ResultCalcModels;
using BackUpInSynch.Models.ResultStructure;
using BackUpInSynch.Models.ScanStructure;
using BackUpInSynch.Utils;

namespace BackUpInSynch.CalculateMissMatches
{
    public class CalculateDiffrences
    {
        public static Issues Issues(string sourceBasePath, string destinationBasePath, DirectoryNode source,
            DirectoryNode dest)
        {
            var output = new Issues
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

            var items = SortDirectories(sourceBasePath, destinationBasePath, source, dest);
            output.DirectoryResultDetailsList.AddRange(items.DirectoryResultDetailsList);
            output.FileResultDetailsList.AddRange(items.FileResultDetailsList);

            // i know this looks like a duplicate but the names being parsed in on last arg switches logic inside method 
            items = SortDirectories(sourceBasePath, destinationBasePath, source, dest, items.NamesMatched);
            output.DirectoryResultDetailsList.AddRange(items.DirectoryResultDetailsList);
            output.FileResultDetailsList.AddRange(items.FileResultDetailsList);

            output.FileResultDetailsList.AddRange(ManageFiles(sourceBasePath, destinationBasePath, source, dest));
            return output;
        }

        private static IEnumerable<FileResultDetails> ManageFiles(string sourceBasePath, string destinationBasePath,
            DirectoryNode source,
            DirectoryNode dest)
        {
            if (source == null && dest == null)
            {
                return new FileResultDetails[0];
            }

            if (source == null)
            {
                return dest.Files.Select(f => ResultActionGenerator.FileGenertor(f, null, sourceBasePath));
            }

            if (dest == null)
            {
                return source.Files.Select(f => ResultActionGenerator.FileGenertor(f, null, destinationBasePath));
            }

            var result = SortFiles(sourceBasePath, destinationBasePath, source, dest);
            var list = result.FileResultDetailsList;
            list.AddRange(SortFiles(sourceBasePath, destinationBasePath, source, dest, result.NamesMatched)
                .FileResultDetailsList);
            return list;
        }

        private static MatchIssue SortFiles(string sourceBasePath, string destinationBasePath,
            DirectoryNode source, DirectoryNode dest, IReadOnlyDictionary<string, bool> namesDone = null)
        {
            var items = new MatchIssue();
            var files = namesDone == null ? source.Files : dest.Files;
            foreach (var file in files)
            {
                var checkFile = namesDone == null ? dest : source;
                var destFile = checkFile.Files.FirstOrDefault(f => f.Name == file.Name);
                items.NamesMatched.Add(file.Name, true);
                if (destFile != null && destFile.Hash == file.Hash)
                {
                    continue;
                }

                var item = ResultActionGenerator.FileGenertor(file, destFile, namesDone == null ? destinationBasePath : sourceBasePath);
                if (namesDone != null)
                {
                    item.Source = false;
                }
                items.FileResultDetailsList.Add(item);
            }

            return items;
        }

        private static MatchIssue SortDirectories(string sourceBasePath, string destinationBasePath,
            DirectoryNode source, DirectoryNode dest, IReadOnlyDictionary<string, bool> namesDone = null)
        {
            var output = new MatchIssue();
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
                    match == dest ? destinationBasePath : sourceBasePath, item, subMatch);

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