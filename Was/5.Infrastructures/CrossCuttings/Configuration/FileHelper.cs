using System;

namespace Kengic.Was.CrossCutting.Configuration
{
    public static class FileHelper
    {
        public static int ConvertFileSize(int baseSize, string sizeUnit)
        {
            var sizeArray = new[] {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "BB", "NB", "DB", "CB"};
            var gap = Array.IndexOf(sizeArray, sizeUnit.ToUpper());
            if (gap == -1)
            {
                throw new ArgumentException($"sizeUnit must in {string.Join(",", sizeArray)}",
                    nameof(sizeUnit));
            }
            return baseSize*(int) Math.Pow(1024.0, gap);
        }
    }
}