using System;
using System.Collections.Generic;
using AlgoLib.Geometry;
using LibSmartCursor.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SmartCursorTweaks.Common.Appliance {
	public class SmartStaffOfRegrowth : SmartCursorAppliance {
		protected override bool IsValidTile(SmartCursorContext ctx, Point point) {
			var config = ModContent.GetInstance<SmartCursorTweaksConfig>();
			if (!config.EnableStaffOfRegrowth || !TileUtils.GetTileSafe(point, out Tile t)) return false;
			if (!t.HasTile) return false;

			int style = t.TileFrameX / 18;

			// If this tile is a blooming herb, return true
			return WorldGen.IsHarvestableHerbWithSeed(t.TileType, style);
		}

		public static ApplianceHandle Register() {
			return LibSmartCursor.LibSmartCursor.Registry.RegisterAppliance(
				item => item.type == ItemID.StaffofRegrowth || item.type == ItemID.AcornAxe,
				new SmartStaffOfRegrowth(),
				SmartCursorRegistry.PRIORITY_LOW
			);
		}
	}
}
