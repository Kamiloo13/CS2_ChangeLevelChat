using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Commands;

namespace ChangeLevelChat;

public static class Colors
{
    public static string Red(string msg) => $" \x02{msg}\x01";
    public static string Purple(string msg) => $" \x03{msg}\x01";
    public static string Green(string msg) => $" \x04{msg}\x01";
    public static string PaleGreen(string msg) => $" \x05{msg}\x01";
    public static string LightGreen(string msg) => $" \x06{msg}\x01";
    public static string PaleRed(string msg) => $" \x07{msg}\x01";
    public static string PalePurple(string msg) => $" \x08{msg}\x01";
    public static string PaleOrange(string msg) => $" \x09{msg}\x01";
    public static string Orange(string msg) => $" \x10{msg}\x01";
    public static string PaleBlue(string msg) => $" \x0B{msg}\x01";
    public static string Blue(string msg) => $" \x0C{msg}\x01";
    public static string Pink(string msg) => $" \x0E{msg}\x01";
    public static string Gray(string msg) => $" \x0A{msg}\x01";
}

public static class PHelpers
{
    public const string AppPrefix = "ChangeLevel";

    public enum LogLevel
    {
        Debug, Info, Error
    }

    private static LogLevel _logLevel = LogLevel.Info;

    public static LogLevel CurrentLogLevel
    {
        get { return _logLevel; }
        set { _logLevel = value; }
    }

    public static void Debug(string message)
    {
        if (_logLevel <= LogLevel.Debug)
            Console.WriteLine($"\x1b[36m[{AppPrefix} DEBUG]\x1b[0m {message}");
    }

    public static void Log(string message)
    {
        if (_logLevel <= LogLevel.Info)
            Console.WriteLine($"\x1b[32m[{AppPrefix} INFO]\x1b[0m {message}");
    }

    public static void Error(string message)
    {
        Console.WriteLine($"\x1b[28m[{AppPrefix} ERROR]\x1b[0m {message}");
        PrintToAll(Colors.PaleRed("An error occured... check server console for more information!"));
    }

    public static void PrintToAll(string message)
    {
        Server.PrintToChatAll(Colors.PaleGreen($"[{AppPrefix}] ") + message);
    }

    public static void ReplyToCmd(CommandInfo commandInfo, string msg)
    {
        commandInfo.ReplyToCommand(Colors.PaleGreen($"[{AppPrefix}] ") + msg);
    }

    public static void SetLogLevel(string logLevel)
    {
        switch (logLevel.ToLower())
        {
            case "info":
                CurrentLogLevel = LogLevel.Info;
                break;
            case "error":
                CurrentLogLevel = LogLevel.Error;
                break;
            case "debug":
                CurrentLogLevel = LogLevel.Debug;
                break;
            default:
                Error("Invalid log level specified");
                break;
        }
    }
}
