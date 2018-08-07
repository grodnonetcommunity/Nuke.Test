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
