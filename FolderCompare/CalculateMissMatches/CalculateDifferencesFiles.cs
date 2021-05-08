using System.Collections.Generic;
using System.Linq;
using FolderCompare.Models.ResultCalcModels;
using FolderCompare.Models.ResultStructure;
using FolderCompare.Models.ScanStructure;
using FolderCompare.Utils;

namespace FolderCompare.CalculateMissMatches
{
    internal static class CalculateDifferencesFiles
    {
        public static IEnumerable<FileResultDetails> ManageFiles(string sourceBasePath, string destinationBasePath,
            DirectoryNode source,
            DirectoryNode dest, bool checkContents)
        {
            if (source == null && dest == null)
            {
                return new FileResultDetails[0];
            }

            if (source == null)
            {
                return dest.Files.Select(f => ResultActionGenerator.FileGenerator(f, null, sourceBasePath));
            }

            if (dest == null)
            {
                return source.Files.Select(f => ResultActionGenerator.FileGenerator(f, null, destinationBasePath));
            }

            var result = SortFiles(sourceBasePath, destinationBasePath, source, dest, checkContents);
            var list = result.FileResultDetailsList;
            
            // parsing in NamesMatched makes the logic with in the method switch source and destination
            list.AddRange(SortFiles(sourceBasePath, destinationBasePath, source, dest, checkContents, result.NamesMatched)
                .FileResultDetailsList);
            return list;
        }
        
        private static MatchResultPotentialIssue SortFiles(string sourceBasePath, string destinationBasePath,
            DirectoryNode source, DirectoryNode dest, bool checkContents, IReadOnlyDictionary<string, bool> namesDone = null)       // names done switches source and dest with in method
        {
            var items = new MatchResultPotentialIssue();
            var files = namesDone == null ? source.Files : dest.Files;
            foreach (var file in files)
            {
                var checkFile = namesDone == null ? dest : source;
                var destFile = checkFile.Files.FirstOrDefault(f => f.Name == file.Name);
                items.NamesMatched.Add(file.Name, true);
                if (destFile != null && destFile.FileInfo.Length == file.FileInfo.Length && (!checkContents || destFile.Hash() == file.Hash()))
                {
                    continue;
                }

                var item = ResultActionGenerator.FileGenerator(file, destFile,
                    namesDone == null ? destinationBasePath : sourceBasePath);
                if (namesDone != null)
                {
                    item.Source = false;
                }

                items.FileResultDetailsList.Add(item);
            }

            return items;
        }
    }
}