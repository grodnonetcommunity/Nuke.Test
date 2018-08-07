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
