using Nuke.Common.Execution;

public class BuildVariablesAttribute : StaticInjectionAttributeBase
{
    public static BuildVariables Value { get; private set; }

    public override object GetStaticValue() => Value ?? (Value = BuildVariables.Parse());
}
