namespace JITK.Core.Command
{
    public enum ArgumentSize
    {
        None, //No argument
        Single,
        Poly, //More argument
        Opt //Optional argument
    }
    public abstract class Command
    {
        abstract public string HelpText { get; }
        abstract public string Name { get; }
        abstract public ArgumentSize ArgSize { get; }
        abstract public void Action();

    }
}
