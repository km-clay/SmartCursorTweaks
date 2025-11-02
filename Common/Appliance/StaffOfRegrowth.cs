using System;
using System.Collections.Generic;
using LibSmartCursor.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SmartCursorRewritten.Common.Appliance {
	public class SmartStaffOfRegrowth : SmartCursorAppliance {
		protected override bool IsValidTile(SmartCursorContext ctx, Point point) {
			Tile t = Main.tile[point.X, point.Y];
			if (!t.HasTile) return false;

			int style = t.TileFrameX / 18;
			return WorldGen.IsHarvestableHerbWithSeed(t.TileType, style);
		}
	}

	public class SmartRegrowthSystem : ModSystem {
		public override void PostSetupContent() {
			LibSmartCursor.LibSmartCursor.Registry.RegisterAppliance(
				item => item.type == Terraria.ID.ItemID.StaffofRegrowth || item.type == Terraria.ID.ItemID.AcornAxe,
				new SmartStaffOfRegrowth(),
				SmartCursorRegistry.PRIORITY_LOW
			);
		}
	}
}
