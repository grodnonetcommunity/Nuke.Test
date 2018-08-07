using static Nuke.Common.IO.PathConstruction;

public static class AbsolutePathExtensions
{
    public static RelativePath ToRelative(this AbsolutePath path, AbsolutePath root)
        => (RelativePath)path.ToString().Substring(root.ToString().Length + 1);
}
