using System;
using System.Collections.Generic;
using BackUpInSynch.Models.ScanStructure;

namespace BackUpInSynch.Models.ResultStructure
{
    public class ResultDetailsBase<T> : EventArgs where T : NodeBase
    {
        public T Data { get; set; }
        
        public T Linked { get; set; }
        
        public IEnumerable<ActionHandlerWithText> ActionHandlerWithTexts { get; set; } 
    }
}