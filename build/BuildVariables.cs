public class BuildVariables
{
    public int Sprint => 1;

    public static BuildVariables Parse()
    {
        return new BuildVariables();
    }
}
