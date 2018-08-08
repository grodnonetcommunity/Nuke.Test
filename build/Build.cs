using System.Linq;
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
        .DependsOn(BackendClean)
        .DependsOn(FrontEndClean)
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Compile => _ => _
        .DependsOn(FrontEndCompile)
        .DependsOn(BackendCompile)
        .DependsOn(BackendUnitTests)
        .DependsOn(FrontEndUnitTests);
}
