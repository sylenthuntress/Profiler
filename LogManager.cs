using BepInEx.Logging;

namespace Profiler;

internal static class LogManager {
    private static ManualLogSource _logSource;

    internal static void Init(ManualLogSource logSource) {
        _logSource = logSource;
        _logSource.LogInfo(Profiler.PluginGUID + "-" + Profiler.PluginVersion + " has loaded!");
    }

    internal static void Debug(object data) => _logSource.LogDebug(data);
    internal static void Error(object data) => _logSource.LogError(data);
    internal static void Fatal(object data) => _logSource.LogFatal(data);
    internal static void Info(object data) => _logSource.LogInfo(data);
    internal static void Message(object data) => _logSource.LogMessage(data);
    internal static void Warning(object data) => _logSource.LogWarning(data);
}
