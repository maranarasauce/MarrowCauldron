[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public class Elixir : Attribute
{
    public static void Log(string message)
    {
        MelonLogger.Msg(message);
    }

    public static void Log(object message)
    {
        MelonLogger.Msg(message);
    }
}