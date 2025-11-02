using LibSmartCursor.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SmartCursorTweaks.Common.Appliance {
	public class SmartRope : SmartCursorAppliance {
		private static int[] ROPE_ITEMS = [
			ItemID.Rope,
			ItemID.SilkRope,
			ItemID.VineRope,
			ItemID.WebRope
		];

		private static int[] ROPE_TILES = [
			TileID.Rope,
			TileID.SilkRope,
			TileID.VineRope,
			TileID.WebRope
		];

		protected override bool IsValidTile(SmartCursorContext ctx, Point pnt) {
			// Get the tile from Main
			Tile tile = Main.tile[pnt.X, pnt.Y];

			// If the tile is a rope, target it
			return tile.HasTile && ROPE_TILES.Contains(tile.TileType);
		}

		public class SmartRopeSystem : ModSystem {
			public override void PostSetupContent() {
				LibSmartCursor.LibSmartCursor.Registry.RegisterAppliance(
					item => ROPE_ITEMS.Contains(item.type),
					new SmartRope(),
					SmartCursorRegistry.PRIORITY_LOW
				);
			}
		}
	}
}
