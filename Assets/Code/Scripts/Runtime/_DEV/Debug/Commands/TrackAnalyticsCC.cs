using UnityEngine;

public class TrackAnalyticsCC : MonoBehaviour, IConsoleCommand
{
    public string CommandString => "analytics";

    public void Execute(DebugConsole caller, params string[] args)
    {
        if (args.Length == 0)
            return;


        string active = args[0].ToLower();

        switch (active)
        {
            case "true":
                AnalyticsTracker.DoTrack = true;
                break;

            case "false":
                AnalyticsTracker.DoTrack = false;
                break;

            default:
                break;
        }
    }
}
