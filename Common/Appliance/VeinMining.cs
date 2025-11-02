using Terraria;
using Terraria.ModLoader;
using static LibSmartCursor.LibSmartCursor;
using LibSmartCursor.API;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;


namespace SmartCursorTweaks.Common.Appliance {
	// This class holds state per-player
	public class VeinMinerPlayer : ModPlayer {
		public int[] Ores = [
			TileID.FossilOre,
			TileID.DesertFossil,
			TileID.Copper,
			TileID.Tin,
			TileID.Iron,
			TileID.Lead,
			TileID.Silver,
			TileID.Tungsten,
			TileID.Gold,
			TileID.Platinum,
			TileID.Demonite,
			TileID.Crimtane,
			TileID.Meteorite,
			TileID.Hellstone,
			TileID.Cobalt,
			TileID.Palladium,
			TileID.Mythril,
			TileID.Orichalcum,
			TileID.Adamantite,
			TileID.Titanium,
			TileID.Chlorophyte,
		];
		public HashSet<Point> CurrentVein { get; set; } = new();
		public int? CurrentVeinOre = null;

		private Point[] GetNeighbors(Point point) => [
			new Point(point.X + 1, point.Y),
			new Point(point.X - 1, point.Y),
			new Point(point.X, point.Y + 1),
			new Point(point.X, point.Y - 1),
			new Point(point.X + 1, point.Y + 1),
			new Point(point.X - 1, point.Y - 1),
			new Point(point.X + 1, point.Y - 1),
			new Point(point.X - 1, point.Y + 1),
		];

		public void ResetVein() {
			CurrentVein.Clear();
			CurrentVeinOre = null;
		}

		public void TraverseVein(Point point) {
			// BFS
			Queue<Point> work = new();
			work.Enqueue(point);

			while (work.Count > 0) {
				Point tile = work.Dequeue();

				if (CurrentVeinOre.HasValue) {
					Tile tCheck = Main.tile[tile.X, tile.Y];
					if (tCheck.HasTile && tCheck.TileType != CurrentVeinOre.Value) {
						continue;
					}
				}
				if (CurrentVein.Contains(tile)) continue;

				Tile t = Main.tile[tile.X, tile.Y];
				if (!t.HasTile || !Ores.Contains(t.TileType)) continue;

				CurrentVein.Add(tile);
				if (CurrentVeinOre == null) {
					CurrentVeinOre = t.TileType;
				}

				foreach (Point n in GetNeighbors(tile)) {
					work.Enqueue(n);
				}
			}
		}
	}

	public class VeinMinerAppliance : SmartCursorAppliance {
		protected override bool IsValidTile(SmartCursorContext ctx, Point tile) {
			// Grab our ModPlayer instance that has our state
			var veinMiner = ctx.Player.GetModPlayer<VeinMinerPlayer>();
			int[] ores = veinMiner.Ores;
			Tile t = Main.tile[tile.X, tile.Y];

			// Use ModPlayer state to make informed decisions
			if (!t.HasTile || !veinMiner.Ores.Contains(t.TileType)) {
				return false;
			}

			if (veinMiner.CurrentVein.Contains(tile)) {
				return true;
			} else if (veinMiner.Ores.Contains(t.TileType) && ((veinMiner.CurrentVeinOre.HasValue && veinMiner.CurrentVeinOre.Value != t.TileType) || !veinMiner.CurrentVein.Contains(tile))) {
				veinMiner.ResetVein();
			}

			if (veinMiner.CurrentVein.Count > 0) {
				return false;
			}

			veinMiner.TraverseVein(tile);
			bool ret = veinMiner.CurrentVein.Contains(tile);
			if (ret) {
				// Force tile to be mineable
				ctx.RestrictedTiles.Remove(tile);
			}
			return ret;
		}
	}

	public class VeinMinerSystem : ModSystem {
		public override void PostSetupContent() {
			Registry.RegisterAppliance(
				item => item.pick > 0,
				new VeinMinerAppliance(),
				SmartCursorRegistry.PRIORITY_NORMAL
			);
		}
	}
}
