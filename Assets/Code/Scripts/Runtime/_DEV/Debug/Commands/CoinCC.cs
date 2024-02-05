using Euphrates;
using UnityEngine;

public class CoinCC : MonoBehaviour, IConsoleCommand
{
    [SerializeField] IntSO _coins;

    public string CommandString => "coin";

    public void Execute(DebugConsole caller, params string[] args)
    {
        int given = args.Length == 0 ? 500 : // If no arguments passed player get 500 coins.
            !int.TryParse(args[0], out int parsed) ? 500 : parsed; // If argument is not a number player gets 500 coins, if an is passsed as an argument player gets asked amount.

        _coins.Value += given;
    }
}
