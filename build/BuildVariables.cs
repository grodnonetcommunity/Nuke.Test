using static Nuke.Common.EnvironmentInfo;

public class BuildVariables
{
    public int Sprint => 2;

    public string Environment => Parameter("environment") ?? "dev";

    public string BuildId => Argument("buildId") ?? Variable("BUILD_ID") ?? "local";

    public bool IsLocal => BuildId == "local";

    public bool IsProd => BuildId == "prod";

    public string Version => Parameter("newVersion");

    public string FileVersion => IsProd || IsLocal ? Version : $"{Version}-{Environment}{Sprint}+build{BuildId}";
}
