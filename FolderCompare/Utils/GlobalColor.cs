using System;
using System.Drawing;

namespace FolderCompare.Utils
{
    internal static class GlobalColor
    {
        public static Color Get(ColorFor item)
        {
            switch (item)
            {
                case ColorFor.SourceInfo:
                    return Color.LimeGreen;
                case ColorFor.DestinationInfo:
                    return Color.Maroon;
                case ColorFor.Window:
                    return Color.Honeydew;
                case ColorFor.Button:
                    return Color.Silver;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }
    }

    internal enum ColorFor
    {
        SourceInfo = 0,
        DestinationInfo = 1,
        Window = 2,
        Button = 4
    }
}