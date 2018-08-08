using System.Linq;
using Nuke.Common;
using Nuke.Common.Tools.MSBuild;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.IO.PathConstruction;

partial class Build : NukeBuild
{
    MSBuildSettings MsBuildSettings => DefaultMSBuild;

    Target BackendClean => _ => _
        .Executes(() =>
        {
            foreach (var project in Solution.Projects.Where(p =>
                p.Directory.ToString() != EnvironmentInfo.BuildProjectDirectory.ToString()))
            {
                DeleteDirectories(GlobDirectories(project.Directory, "bin", "obj"));
            }
        });

    Target BackendRestore => _ => _
        .DependsOn(BackendClean)
        .Executes(() =>
        {
            MSBuild(s => MsBuildSettings
                .SetTargets("Restore"));
        });

    Target BackendCompile => _ => _
        .DependsOn(BackendRestore)
        .Executes(() =>
        {
            MSBuild(s => MsBuildSettings);
        });

    Target BackendPack => _ => _
        .DependsOn(BackendCompile)
        .Executes(() =>
        {
            MSBuild(s => MsBuildSettings
                .SetTargets("Restore", "Pack")
                .SetPackageOutputPath(ArtifactsDirectory)
                .SetConfiguration(Configuration)
                .EnableIncludeSymbols());
        });
}
