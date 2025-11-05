using LibSmartCursor.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using AlgoLib.Geometry;

namespace SmartCursorTweaks.Common.Appliance {
	public class ShinyFinder : SmartCursorAppliance {
		private int[] Shinies = [
			TileID.Crystals,
			TileID.ExposedGems
		];

		protected override bool IsValidTile(SmartCursorContext ctx, Point pnt) {
			var config = ModContent.GetInstance<SmartCursorTweaksConfig>();
			if (!config.EnableTargetCrystals || !TileUtils.GetTileSafe(pnt, out Tile tile)) return false;

			return tile.HasTile && Shinies.Contains(tile.TileType);
		}
		public static ApplianceHandle Register() {
			return LibSmartCursor.LibSmartCursor.Registry.RegisterAppliance(
				item => item.pick > 0,
				new ShinyFinder(),
				SmartCursorRegistry.PRIORITY_HIGH
			);
		}
	}
}
