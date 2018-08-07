using System;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.MSBuild;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

partial class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    public override AbsolutePath SourceDirectory => RootDirectory / "src";

    Target Clean => _ => _
        .Executes(() =>
        {
            DeleteDirectories(GlobDirectories(SourceDirectory, "**/bin", "**/obj"));
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(SolutionFile)
                .SetTargets("Restore"));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(SolutionFile)
                .SetTargets("Rebuild")
                .SetConfiguration(Configuration)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .SetNodeReuse(IsLocalBuild));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(SolutionFile)
                .SetTargets("Restore", "Pack")
                .SetPackageOutputPath(ArtifactsDirectory)
                .SetConfiguration(Configuration)
                .EnableIncludeSymbols());
        });
}
