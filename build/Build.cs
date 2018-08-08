using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

partial class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [BuildVariables] readonly BuildVariables BuildVariables;

    public override AbsolutePath SourceDirectory => RootDirectory / "src";

    Target Clean => _ => _
        .Executes(() =>
        {
            foreach (var project in Solution.Projects.Where(p => p.Directory.ToString() != EnvironmentInfo.BuildProjectDirectory.ToString()))
            {
                DeleteDirectories(GlobDirectories(project.Directory, "bin", "obj"));
            }
            EnsureCleanDirectory(ArtifactsDirectory);
        });
}
