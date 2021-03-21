using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackUpInSynch.Models.ResultStructure;
using BackUpInSynch.Models.ScanStructure;

namespace BackUpInSynch.Utils
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
            return new DirectoryResultDetails
            {
                Data = source,
                ActionHandlerWithTexts = new List<ActionHandlerWithText>
                {
                    Generate("Copy Me",()=> 
                        FileAndIoUtils.DirectoryCopy(source.FullLocation, Path.Combine(destinationBasePath, source.RelativeLocation), true) ),
                    Generate("Delete Me",()=>Directory.Delete(source.FullLocation))
                }
            };
        }
        
        public static FileResultDetails FileGenertor(FileNode source,FileNode dest , string destinationBasePath)
        {
            var item = new FileResultDetails
            {
                Data = source,
                Linked = dest,
                ActionHandlerWithTexts = dest == null ? new List<ActionHandlerWithText>() : new List<ActionHandlerWithText>
                {
                    Generate("OverWriteThem", () => File.Copy(source.FullLocation,dest.FullLocation, true))
                }
            };
            

            item.ActionHandlerWithTexts = item.ActionHandlerWithTexts.Union(new List<ActionHandlerWithText>
            {
                Generate("Copy Me",
                    () => File.Copy(source.FullLocation,
                        Path.Combine(destinationBasePath, source.RelativeLocation), true)),
                Generate("Delete Me", () => File.Delete(source.FullLocation))
            });

            return item;
        }
    }
}