using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Tools.Npm.NpmTasks;
using static Nuke.Common.IO.PathConstruction;

partial class Build
{
    AbsolutePath WebApp => SourceDirectory / "webapp";

    Target FrontEndClean => _ => _
        .Executes(() =>
        {
            NpmRun(s => s.SetWorkingDirectory(WebApp).SetCommand("clean"));
        });

    Target FrontEndNpmInstall => _ => _
        .Executes(() =>
        {
            NpmInstall(s => s.SetWorkingDirectory(WebApp));
        });

    Target FrontEndCompile => _ => _
        .DependsOn(FrontEndClean)
        .DependsOn(FrontEndNpmInstall)
        .Executes(() =>
        {
            NpmRun(s => s.SetWorkingDirectory(WebApp).SetCommand("build"));
        });
}