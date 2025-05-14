using System;
using System.Collections.Generic;
using System.Linq;
using Profiler.Utils;
using Rewired;
using RoR2;
using Zio;

namespace Profiler.Features;

internal static class DefaultProfileSelector {
    internal const string NameVariable = "[username]";
    internal const string OriginalVariable = "[original]";
    
    public static void SelectDefaultProfile(On.RoR2.LocalUserManager.orig_AddUser orig, Player player, UserProfile userProfile) {
        string startupProfileName = ParseProfileName(ConfigManager.StartupProfile.Value);
        if (string.IsNullOrEmpty(startupProfileName)) {
            LogManager.Info("No startup profile found. Defaulting...");
            orig(player, userProfile); // The original method
            return;
        }
        
        UserProfile startupProfile = null;
        LogManager.Info("The following save files were detected:");
        foreach (var profile in PlatformSystems.saveSystem.loadedUserProfiles.Select(entry => entry.Value)) {
            LogManager.Info("   - {} ({})", profile.name, profile.fileName);
            if (profile.name == startupProfileName) {
                startupProfile = profile;
            }
        }
        
        if (startupProfile == null) {
            if (ConfigManager.CreateStartupProfileIfAbsent.Value) {
                startupProfile = PlatformSystems.saveSystem.CreateProfile(RoR2Application.fileSystem, startupProfileName);
                LogManager.Warning("Profile {} not found. Fallback profile creation is enabled. Creating...", startupProfileName);
            } else {
                LogManager.Warning("Profile {} not found. Fallback profile creation is disabled. Defaulting...", startupProfileName);
                orig(player, userProfile); // The original method
                return;
            }
        }
        
        LogManager.Info("Profile {} found! Selecting...", startupProfileName);
        orig(player, startupProfile);
    }

    private static string ParseProfileName(string name) {
        if (string.IsNullOrEmpty(name)) {
            return name;
        }
        
        return name.Replace(NameVariable, PlatformSystems.saveSystem.GetPlatformUsernameOrDefault("Nameless Survivor"))
            .Replace(OriginalVariable, GetFirstProfile().name);
    }

    private static UserProfile GetFirstProfile() {
        UserProfile userProfile = null;
        DateTime oldestCreationTime = DateTime.Now;
        foreach (var entry in PlatformSystems.saveSystem.loadedUserProfiles) {
            UserProfile profile = entry.Value;
            DateTime creationTime = profile.fileSystem.GetFileEntry(profile.filePath).CreationTime;
            if (oldestCreationTime.CompareTo(creationTime) < 0) {
                userProfile = profile;
                oldestCreationTime = creationTime;
            }
        }

        return userProfile;
    }
}