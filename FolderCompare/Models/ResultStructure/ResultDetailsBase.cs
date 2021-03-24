using System;
using System.Collections.Generic;
using FolderCompare.Models.ScanStructure;

namespace FolderCompare.Models.ResultStructure
{
    public class ResultDetailsBase<T> : EventArgs where T : NodeBase
    {
        public bool Source = true;
        public T Data { get; set; }

        public T Linked { get; set; }

        public IEnumerable<ActionHandlerWithText> ActionHandlerWithTexts { get; set; }
    }
}