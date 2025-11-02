using LibSmartCursor.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SmartCursorTweaks.Common.Appliance {
	public class HellevatorGuide : SmartCursorAppliance {
		protected override bool IsValidTile(SmartCursorContext ctx, Point pnt) {
			// Only target tiles below the player
			if (pnt.Y < (ctx.Player.Center.Y / 16f)) {
				return false;
			}

			// Don't target if the mouse is above the tile
			if (pnt.Y > (Main.MouseWorld.Y / 16f)) {
				return false;
			}
			// Don't target if in the overworld
			if (ctx.Player.ZoneOverworldHeight) {
				return false;
			}

			// Check for a lot of open vertical space above the tile
			for (int i = 1; i < 300; i++) {
				if (!WorldGen.InWorld(pnt.X, pnt.Y - i)) {
					break;
				}
				Tile tile = Main.tile[pnt.X, pnt.Y - i];
				if (tile.HasTile && Main.tileSolid[tile.TileType]) {
					return false;
				}
			}

			// There is a lot of open vertical space above this tile. Certainly a hellevator.
			Tile scrutinee = Main.tile[pnt.X, pnt.Y];

			return scrutinee.HasTile;
		}

		public class HellevatorGuideSystem : ModSystem {
			public override void PostSetupContent() {
				LibSmartCursor.LibSmartCursor.Registry.RegisterAppliance(
					item => item.pick > 0,
					new HellevatorGuide(),
					SmartCursorRegistry.PRIORITY_HIGH // override pickaxe appliances
				);
			}
		}
	}
}
