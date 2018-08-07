# Nuke Demo

## Step 1

Create solution

```cmd
mkdir src
cd src
dotnet new solution -n Nuke.Test
dotnet new webapi -n TestApi
dotnet sln add TestApi
```

Build application and run

```cmd
dotnet build
dotnet run --project TestApi
start http://localhost:5000/api/values
```

## Step 2

Let's use `nuke`

Install `nuke` as global tool

```cmd
dotnet tool install -g Nuke.GlobalTool
nuke
```

Output:

```
>nuke
Could not find .nuke file. Do you want to setup a build? [y/n]
Which solution should be the default?
?  src\Nuke.Test.sln
How should the build project be bootstrapped?
?  .NET Core SDK
   .NET Framework/Mono
What target framework should be used?
?  netcoreapp2.0
   net461
Which NUKE version should be used?
?  0.6.0 (latest release)
   0.5.3 (latest local)
Where should the build project be located?
?
   ./build         ? for default
What should be the name for the build project?
?
   _build          ? for default
build
Do you need help getting started with a basic build?
?  Yes, get me started!
   No, I can do this myself...
Restore, compile, pack using ...
   dotnet CLI
?  MSBuild/Mono
   None of both
Source files are located in ...
   ./source
?  ./src
   None of both
Move packages to ...
   ./output
?  ./artifacts
   None of both
Where do test projects go?
   ./tests
?  Same where source goes
Do you use GitVersion?
   Yes, just not setup yet
?  No, custom versioning
Creating directory 'D:\Projects\Nuke.Test\.\build'...
```

Run `nuke` one more time

```cmd
nuke
```

Output

```
>nuke
Windows PowerShell 5.1.17134.165
Microsoft (R) .NET Core SDK version 2.1.300
Build.cs(19,18): warning CS0114: 'Build.SourceDirectory' hides inherited member 'NukeBuild.SourceDirectory'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword. [D:\Projects\Nuke.Test\build\build.csproj]
Build.cs(20,18): warning CS0114: 'Build.ArtifactsDirectory' hides inherited member 'NukeBuild.ArtifactsDirectory'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword. [D:\Projects\Nuke.Test\build\build.csproj]

_  _ _  _ _  _ ____
|\ | |  | |_/  |___
| \| |__| | \_ |___

Version: 0.6.0 [CommitSha: 5a428f0d]
Host: Console

> "C:\Program Files\Git\cmd\git.exe" rev-parse --abbrev-ref HEAD
Assertion failed: Could not parse remote URL for 'origin'.


____ _    ____ ____ _  _
|    |    |___ |__| |\ |
|___ |___ |___ |  | | \|

Deleting directory 'D:\Projects\Nuke.Test\src\TestApi\bin'...
Deleting directory 'D:\Projects\Nuke.Test\src\TestApi\obj'...
Creating directory 'D:\Projects\Nuke.Test\artifacts'...

____ ____ ____ ___ ____ ____ ____
|__/ |___ [__   |  |  | |__/ |___
|  \ |___ ___]  |  |__| |  \ |___

> "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\amd64\msbuild.exe" D:\Projects\Nuke.Test\src\Nuke.Test.sln /target:Restore
Microsoft (R) Build Engine version 15.7.179.6572 for .NET Framework
Copyright (C) Microsoft Corporation. All rights reserved.

Building the projects in this solution one at a time. To enable parallel build, please add the "/m" switch.
Build started 07.08.2018 22:01:50.
Project "D:\Projects\Nuke.Test\src\Nuke.Test.sln" on node 1 (Restore target(s)).
ValidateSolutionConfiguration:
  Building solution configuration "Debug|Any CPU".
Restore:
  Restoring packages for D:\Projects\Nuke.Test\src\TestApi\TestApi.csproj...
  Committing restore...
  Generating MSBuild file D:\Projects\Nuke.Test\src\TestApi\obj\TestApi.csproj.nuget.g.props.
  Generating MSBuild file D:\Projects\Nuke.Test\src\TestApi\obj\TestApi.csproj.nuget.g.targets.
  Writing lock file to disk. Path: D:\Projects\Nuke.Test\src\TestApi\obj\project.assets.json
  Restore completed in 2,46 sec for D:\Projects\Nuke.Test\src\TestApi\TestApi.csproj.

  NuGet Config files used:
      C:\Users\werwolf\AppData\Roaming\NuGet\NuGet.Config
      C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.Offline.config

  Feeds used:
      https://api.nuget.org/v3/index.json
      C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\
Done Building Project "D:\Projects\Nuke.Test\src\Nuke.Test.sln" (Restore target(s)).

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:03.28

____ ____ _  _ ___  _ _    ____
|    |  | |\/| |__] | |    |___
|___ |__| |  | |    | |___ |___

> "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\amd64\msbuild.exe" D:\Projects\Nuke.Test\src\Nuke.Test.sln /maxcpucount:8 /nodeReuse:True /p:Configuration=Debug /target:Rebuild
Microsoft (R) Build Engine version 15.7.179.6572 for .NET Framework
Copyright (C) Microsoft Corporation. All rights reserved.

Build started 07.08.2018 22:01:53.
   ----- skipped lines -----
Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:02.17

Repeating warnings and errors:
Assertion failed: Could not parse remote URL for 'origin'.

========================================
Target              Status      Duration
----------------------------------------
Clean               Executed        0:00
Restore             Executed        0:03
Compile             Executed        0:02
----------------------------------------
Total                               0:05
========================================

Build succeeded on 07.08.2018 22:01:55.
```

Check `./artifacts` folder it is empty.

Because default nuke target doesn't copy artifacts, it just build the solution.

