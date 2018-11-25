using HBS.DebugConsole;
using HBS.Scripting.Attributes;
using Necro;

namespace ThisIsASwordsUncutEquipment
{
    [ScriptBinding("")]
    public class UncutEquipmentConsoleCommand
    {
        [ScriptBinding]
        public static void UncutEquipment()
        {
            DebugConsole.Log("RAIN UNCUT EQUIPMENT!");
            ConsoleCommands.SpawnLootItem("AmbassadorGreatsword");
            ConsoleCommands.SpawnLootItem("GreatswordTuln");
            ConsoleCommands.SpawnLootItem("SkullHammer");
            ConsoleCommands.SpawnLootItem("HealingHam");
            ConsoleCommands.SpawnLootItem("MaxwellHammer");
            ConsoleCommands.SpawnLootItem("ThunderHammer");
            ConsoleCommands.SpawnLootItem("SwordOfAli");
            ConsoleCommands.SpawnLootItem("AbraxisWand");
            ConsoleCommands.SpawnLootItem("BaseShield2");
            ConsoleCommands.SpawnLootItem("BerelaineShield");
            ConsoleCommands.SpawnLootItem("StaffOfPoints");
            ConsoleCommands.SpawnLootItem("GoldBuckler");
            ConsoleCommands.SpawnLootItem("Stilletto");
            ConsoleCommands.SpawnLootItem("ProngedHilt");
            ConsoleCommands.SpawnLootItem("ProngedBlade");
            ConsoleCommands.SpawnLootItem("IntricateShield");
            ConsoleCommands.SpawnLootItem("UnreliableBuckler");
            ConsoleCommands.SpawnLootItem("LargeWoodenShield");
            ConsoleCommands.SpawnLootItem("ShellMaceHammer");
            ConsoleCommands.SpawnLootItem("ThermiasBlade");
            ConsoleCommands.SpawnLootItem("WingedBlade");
            ConsoleCommands.SpawnLootItem("SpectralShiv");
            ConsoleCommands.SpawnLootItem("VikingAxe");

        }
    }
}
