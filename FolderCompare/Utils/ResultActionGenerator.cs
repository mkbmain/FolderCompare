using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FolderCompare.Models.ResultStructure;
using FolderCompare.Models.ScanStructure;

namespace FolderCompare.Utils
{
    public static class ResultActionGenerator
    {
        private static ActionHandlerWithText Generate(string text, Action action)
        {
            return new ActionHandlerWithText
            {
                Text = text,
                Action = action
            };
        }

        public static DirectoryResultDetails FolderGenerator(DirectoryNode source, string destinationBasePath)
        {
            var copyPath = Path.Combine(destinationBasePath, source.RelativeLocation);
            return new DirectoryResultDetails
            {
                Data = source,
                ActionHandlerWithTexts = new List<ActionHandlerWithText>
                {
                    Generate("Copy Me", () => FileAndIoUtils.DirectoryCopy(source.FullLocation, copyPath)),
                    Generate("Delete Me", () => Directory.Delete(source.FullLocation, true))
                }
            };
        }

        public static FileResultDetails FileGenerator(FileNode source, FileNode dest, string destinationBasePath)
        {
            var actionHandlers = dest == null
                ? new List<ActionHandlerWithText>
                {
                    Generate("Copy Me",
                        () =>
                        {
                            var locationForNewFile = Path.Combine(destinationBasePath, source.RelativeLocation);
                            var directory = locationForNewFile.Substring(0, locationForNewFile.Length - source.Name.Length);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            File.Copy(source.FullLocation, locationForNewFile, true);
                        })
                }
                : new List<ActionHandlerWithText>
                {
                    Generate("OverWriteThem", () => File.Copy(source.FullLocation, dest.FullLocation, true)),
                    Generate("OverWriteMe", () => File.Copy(dest.FullLocation, source.FullLocation, true))
                };

            actionHandlers.Add(Generate("Delete Me", () => File.Delete(source.FullLocation)));

            return new FileResultDetails
            {
                Data = source,
                Linked = dest,
                ActionHandlerWithTexts = actionHandlers
            };
        }
    }
}