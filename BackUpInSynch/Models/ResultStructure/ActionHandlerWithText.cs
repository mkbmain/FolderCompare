using System;

namespace BackUpInSynch.Models.ResultStructure
{
    public class ActionHandlerWithText
    {
        public override string ToString()
        {
            return Text;
        }

        public Action Action { get; set; }
        public string Text { get; set; }
    }
}