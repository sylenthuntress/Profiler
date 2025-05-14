using BepInEx;
using Profiler.Features;
using Profiler.Utils;

namespace Profiler;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)] // Metadata

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
        On.RoR2.LocalUserManager.AddUser += DefaultProfileSelector.SelectDefaultProfile;
    }
    
    private void OnDisable() {
        On.RoR2.LocalUserManager.AddUser -= DefaultProfileSelector.SelectDefaultProfile;
    }
}