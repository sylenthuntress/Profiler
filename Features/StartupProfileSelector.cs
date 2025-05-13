using Profiler.Utils;
using Rewired;
using RoR2;

namespace Profiler.Features;

public class StartupProfileSelector { 
    public static void SelectStartupProfile(On.RoR2.LocalUserManager.orig_AddUser orig, Player player, UserProfile profile) {
        string startupProfileName = ConfigManager.StartupProfile.Value;
        if (string.IsNullOrEmpty(startupProfileName)) {
            orig(player, profile); // The original method
            return;
        }
        
        UserProfile startupProfile = PlatformSystems.saveSystem.GetProfile(startupProfileName);
        if (startupProfile == null) {
            if (ConfigManager.CreateStartupProfileIfAbsent.Value) {
                startupProfile = PlatformSystems.saveSystem.CreateProfile(RoR2Application.cloudStorage, startupProfileName);
                LogManager.Warning("Profile {} not found", startupProfile);
            }
        }
        
        orig(player, startupProfile);
    }
}