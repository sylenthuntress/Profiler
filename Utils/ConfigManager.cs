using BepInEx.Configuration;

namespace Profiler.Utils;

internal static class ConfigManager {
    public static ConfigEntry<string> StartupProfile { get; private set; }
    public static ConfigEntry<bool> CreateStartupProfileIfAbsent { get; private set; }
    
    public static void Init(ConfigFile config) {
        StartupProfile = config.Bind<string>(
            "Startup",
            "StartupProfile",
            null,
            "Which profile to select on startup"
        );
        
        CreateStartupProfileIfAbsent = config.Bind(
            "Startup",
            "CreateStartupProfileIfAbsent",
            true,
            "Create startup profile if absent"
        );
    }
}