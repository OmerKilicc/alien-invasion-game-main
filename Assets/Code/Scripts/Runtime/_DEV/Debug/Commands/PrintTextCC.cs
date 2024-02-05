using UnityEngine;

public class PrintTextCC : MonoBehaviour, IConsoleCommand
{
    public string CommandString => "print";

    public void Execute(DebugConsole caller, params string[] args)
    {
        if (args.Length == 0)
            return;

        string msg = string.Empty;

        for (int i = 0; i < args.Length; i++)
            msg += args[i] + ' ';

        print(msg);
    }
}
