using System;
using System.Linq;
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

    MSBuildSettings MsBuildSettings => DefaultMSBuild;

    Target Clean => _ => _
        .Executes(() =>
        {
            foreach (var project in Solution.Projects.Where(p => p.Directory.ToString() != EnvironmentInfo.BuildProjectDirectory.ToString()))
            {
                DeleteDirectories(GlobDirectories(project.Directory, "bin", "obj"));
            }
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            MSBuild(s => MsBuildSettings
                .SetTargets("Restore"));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            MSBuild(s => MsBuildSettings);
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            MSBuild(s => MsBuildSettings
                .SetTargets("Restore", "Pack")
                .SetPackageOutputPath(ArtifactsDirectory)
                .SetConfiguration(Configuration)
                .EnableIncludeSymbols());
        });
}
