using LibSmartCursor.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using AlgoLib.Geometry;

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
			if (!TileUtils.GetTileSafe(pnt, out Tile tile)) return false;

			// If the tile is a rope, target it
			return tile.HasTile && ROPE_TILES.Contains(tile.TileType);
		}

		public static ApplianceHandle Register() {
			return LibSmartCursor.LibSmartCursor.Registry.RegisterAppliance(
				item => ROPE_ITEMS.Contains(item.type),
				new SmartRope(),
				SmartCursorRegistry.PRIORITY_LOW
			);
		}
	}
}
