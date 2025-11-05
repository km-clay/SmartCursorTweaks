using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using Terraria.ModLoader;

namespace SmartCursorTweaks {
	public class SmartCursorTweaksConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header($"$Mods.SmartCursorTweaks.Config.Headers.Pickaxe")]

		[DefaultValue(true)]
		public bool EnableHellevatorGuide { get; set; }

		[DefaultValue(true)]
		public bool EnableVeinMining { get; set; }

		[DefaultValue(true)]
		public bool EnableTargetCrystals { get; set; }

		[Header($"$Mods.SmartCursorTweaks.Config.Headers.Misc")]
		[DefaultValue(true)]
		public bool EnableRopePlacement { get; set; }

		[DefaultValue(true)]
		public bool EnableStaffOfRegrowth { get; set; }
/*
HellevatorGuide.cs
Rope.cs
StaffOfRegrowth.cs
TargetCrystals.cs
VeinMining.cs
*/
	}
}
