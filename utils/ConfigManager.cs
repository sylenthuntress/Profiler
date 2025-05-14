using BepInEx.Configuration;
using Profiler.Features;

namespace Profiler.Utils;

internal static class ConfigManager {
    public static ConfigEntry<string> StartupProfile { get; private set; }
    public static ConfigEntry<bool> CreateStartupProfileIfAbsent { get; private set; }
    
    public static void Init(ConfigFile config) {
        StartupProfile = config.Bind<string>(
            "Startup",
            "DefaultProfile",
            null,
            "Which profile to select on startup." +
            " " + DefaultProfileSelector.NameVariable + " parses to the user's username;" +
            " " + DefaultProfileSelector.OriginalVariable + " parses to the oldest save profile"
        );
        
        CreateStartupProfileIfAbsent = config.Bind(
            "Startup",
            "CreateDefaultProfileIfAbsent",
            true,
            "Create default profile on startup if absent"
        );
    }
}