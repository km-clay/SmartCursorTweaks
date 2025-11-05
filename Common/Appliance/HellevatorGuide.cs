using LibSmartCursor.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using AlgoLib.Geometry;

namespace SmartCursorTweaks.Common.Appliance {
	public class HellevatorGuide : SmartCursorAppliance {
		protected override bool IsValidTile(SmartCursorContext ctx, Point pnt) {
			var config = ModContent.GetInstance<SmartCursorTweaksConfig>();
			Point playerPos = GridUtils.WorldToTile(ctx.Player.Center);
			Point mousePos = GridUtils.WorldToTile(Main.MouseWorld);
			if (!config.EnableHellevatorGuide) return false;

			// Only target tiles below the player
			if (pnt.Y < playerPos.Y) {
				return false;
			}

			// Don't target if the mouse is above the tile
			if (pnt.Y > mousePos.Y) {
				return false;
			}
			// Don't target if in the overworld
			if (ctx.Player.ZoneOverworldHeight) {
				return false;
			}

			// Check for a lot of open vertical space above the tile
			for (int i = 1; i < 150; i++) {
				if (!TileUtils.GetTileSafe(pnt.X, pnt.Y - i, out Tile tile)) break;
				if (tile.HasTile && Main.tileSolid[tile.TileType]) {
					return false;
				}
			}

			// There is a lot of open vertical space above this tile. Certainly a hellevator.
			if (!TileUtils.GetTileSafe(pnt, out Tile scrutinee)) return false;

			return scrutinee.HasTile;
		}

		public static ApplianceHandle Register() {
			return LibSmartCursor.LibSmartCursor.Registry.RegisterAppliance(
				item => item.pick > 0,
				new HellevatorGuide(),
				SmartCursorRegistry.PRIORITY_HIGH // override pickaxe appliances
			);
		}
	}
}
