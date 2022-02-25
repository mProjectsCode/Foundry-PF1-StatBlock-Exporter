namespace Foundry_PF1_StatBlock_Exporter
{
    public static class PathHelper
    {
        public static string Combine(string path1, string path2)
        {
            path1 = TrimSlashes(path1);
            path2 = TrimSlashes(path2);

            return path1 + "/" + path2;
        }

        public static string TrimSlashes(string path)
        {
            path = ConvertSlashes(path);

            if (path.EndsWith("/"))
            {
                path = path[..^1];
            }

            if (path.StartsWith("/"))
            {
                path = path[1..];
            }

            return path;
        }

        public static string ConvertSlashes(string path)
        {
            path = path.Replace("\\", "/");

            return path;
        }
    }
}