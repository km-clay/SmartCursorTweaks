using Terraria.ModLoader;
using LibSmartCursor.API;
using System.Collections.Generic;
using System;

namespace SmartCursorTweaks.Common.Appliance {
	public class ApplianceManager : ModSystem {
		internal static Dictionary<string, ApplianceHandle> appliances = new();

		private void SyncAppliance(string key, bool enabled, Func<ApplianceHandle> register) {
			if (enabled) {
				if (!appliances.ContainsKey(key)) {
					var handle = register();
					appliances[key] = handle;
				}
			} else {
				RemoveAppliance(key);
			}
		}

		public override void PostUpdatePlayers() {
			var config = ModContent.GetInstance<SmartCursorTweaksConfig>();
			SyncAppliance("SmartRope", config.EnableRopePlacement, SmartRope.Register);
			SyncAppliance("HellevatorGuide", config.EnableHellevatorGuide, HellevatorGuide.Register);
			SyncAppliance("SmartStaffOfRegrowth", config.EnableStaffOfRegrowth, SmartStaffOfRegrowth.Register);
			SyncAppliance("VeinMining", config.EnableVeinMining, VeinMinerAppliance.Register);
			SyncAppliance("ShinyFinder", config.EnableTargetCrystals, ShinyFinder.Register);
		}

		private void RemoveAppliance(string key) {
			if (appliances.ContainsKey(key))  {
				var handle = appliances[key];
				LibSmartCursor.LibSmartCursor.Registry.UnregisterAppliance(handle);
				appliances.Remove(key);
			}
		}
	}
}
