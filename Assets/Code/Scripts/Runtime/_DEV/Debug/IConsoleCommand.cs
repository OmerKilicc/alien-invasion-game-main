public interface IConsoleCommand
{
    public string CommandString { get; }
    public void Execute(DebugConsole caller, params string[] args);
}
