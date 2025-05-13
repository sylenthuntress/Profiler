using BepInEx;
using R2API;
using Rewired;
using RoR2;

namespace Profiler;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)] // Metadata
[BepInDependency(LanguageAPI.PluginGUID)] // Language API

public class Profiler : BaseUnityPlugin {
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "SylentHuntress";
    public const string PluginName = "Profiler";
    public const string PluginVersion = "1.0.0";

    // The Awake() method is run at the very start when the game is initialized.
    public void Awake() {
        LogManager.Init(Logger);
        ConfigManager.Init(Config);
    }

    private void OnEnable() {
        On.RoR2.LocalUserManager.AddUser += AlterDefaultProfile;
    }
    
    private void OnDisable() {
        On.RoR2.LocalUserManager.AddUser -= AlterDefaultProfile;
    }
    
    private static void AlterDefaultProfile(On.RoR2.LocalUserManager.orig_AddUser orig, Player player, UserProfile profile) {
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