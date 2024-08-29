using BepInEx;
using HarmonyLib;
using VoiceOfTheCommunity.CustomPatches;

namespace VoiceOfTheCommunity;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        Harmony.CreateAndPatchAll(typeof(CustomItemsPatch));
        Harmony.CreateAndPatchAll(typeof(OberonPatch));
        Harmony.CreateAndPatchAll(typeof(ReassemblyPatch));
        Harmony.CreateAndPatchAll(typeof(JumpHeightPatch));
        Harmony.CreateAndPatchAll(typeof(ArmsPatch));
        Harmony.CreateAndPatchAll(typeof(ItemPurchasePatch));
        Harmony.CreateAndPatchAll(typeof(ItemDestroyPatch));
        Harmony.CreateAndPatchAll(typeof(SpiritOfNegotiatorPatch));
        Logger.LogInfo($"Mod {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}