using System;
using System.Collections.Generic;
using System.Text;

namespace KMS.src.tool
{
    internal static class Toolset
    {
        internal static string GetParentPath(string path)
        {
            if (path == null || path.Length == 0)
                return path;

            int lio = path.LastIndexOf('/');
            if (lio > 0)
            {
                return path.Substring(0, lio);
            }
            else
            {
                lio = path.LastIndexOf('\\');
                if (lio > 0)
                    return path.Substring(0, lio);
                else
                    return path;
            }
        }

        internal static string GetBasename(string path)
        {
            if (path == null || path.Length == 0)
                return "";

            int lio = path.LastIndexOf('/');
            if (lio < 0)
            {
                lio = path.LastIndexOf('\\');
                if (lio < 0)
                    return path;
                else if (lio < path.Length - 1)
                    return path.Substring(lio + 1);
                else
                    return "";
            }
            else
            {
                if (lio < path.Length - 1)
                    return path.Substring(lio + 1);
                else
                    return "";
            }
        }
    }
}
