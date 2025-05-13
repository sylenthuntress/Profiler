using System.Linq;
using Profiler.Utils;
using R2API.Utils;
using Rewired;
using RoR2;

namespace Profiler.Features;

internal static class StartupProfileSelector {
    internal const string NameVariable = "[username]";
    internal const string OriginalVariable = "[original]";
    
    public static void SelectStartupProfile(On.RoR2.LocalUserManager.orig_AddUser orig, Player player, UserProfile profile) {
        string startupProfileName = ParseProfileName(ConfigManager.StartupProfile.Value);
        if (string.IsNullOrEmpty(startupProfileName)) {
            LogManager.Info("No startup profile found, defaulting...");
            orig(player, profile); // The original method
            return;
        }
        
        UserProfile startupProfile = PlatformSystems.saveSystem.GetProfile(startupProfileName);
        if (startupProfile == null) {
            if (ConfigManager.CreateStartupProfileIfAbsent.Value) {
                startupProfile = PlatformSystems.saveSystem.CreateProfile(RoR2Application.cloudStorage, startupProfileName);
                LogManager.Warning("Profile {} not found, creating...", startupProfileName);
            } else LogManager.Warning("Profile {} not found. Cannot create; defaulting...", startupProfileName);
        }
        
        orig(player, startupProfile);
    }

    private static string ParseProfileName(string name) {
        return name.Replace(NameVariable, PlatformSystems.saveSystem.GetPlatformUsernameOrDefault("Nameless Survivor"))
            .Replace(OriginalVariable, PlatformSystems.saveSystem.loadedUserProfiles.First().Key);
    }
}