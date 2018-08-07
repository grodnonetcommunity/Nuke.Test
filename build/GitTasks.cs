using System.Collections.Generic;
using static Nuke.Common.Tools.Git.GitTasks;

public static class GitTasks
{
    public static void GitAdd(string workingDirectory, params string[] files)
        => GitAdd(workingDirectory, (IEnumerable<string>)files);

    public static void GitAdd(string workingDirectory, IEnumerable<string> files)
        => Git($"add {string.Join(" ", files)}", workingDirectory);

    public static void GitCommit(string workingDirectory, string message)
        => Git($"commit -m \"{message}\"", workingDirectory);
}

