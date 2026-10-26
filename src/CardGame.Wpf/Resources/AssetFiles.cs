using System.IO;

namespace CardGame.Wpf.Resources;

/// <summary>Resolves shipped assets independently of the working directory and source checkout.</summary>
public static class AssetFiles
{
    public static string GetPath(string localPath)
    {
        const string sourcePrefix = "src/CardGame.Wpf/";
        var relative = localPath.Replace('\\', '/');
        if (relative.StartsWith(sourcePrefix, StringComparison.Ordinal)) relative = relative[sourcePrefix.Length..];
        if (!relative.StartsWith("Assets/", StringComparison.Ordinal) ||
            relative.Split('/').Any(part => part.Length == 0 || part is "." or ".." ||
                part.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0))
            throw new InvalidDataException("Asset paths must name a file inside the local Assets directory.");

        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Assets")) + Path.DirectorySeparatorChar;
        var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, relative));
        if (!path.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("Asset path escapes the local Assets directory.");
        return path;
    }
}
