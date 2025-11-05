using Terraria;
using Terraria.ModLoader;
using static LibSmartCursor.LibSmartCursor;
using LibSmartCursor.API;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AlgoLib.Algo.Line;
using AlgoLib.Geometry;
using AlgoLib.Algo.Graph;
using System;


namespace SmartCursorTweaks.Common.Appliance {
public class VeinMinerAppliance : SmartCursorAppliance {
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
			TileID.Diamond,
			TileID.Ruby,
			TileID.Emerald,
			TileID.Sapphire,
			TileID.Amethyst,
			TileID.Topaz,
		];
		public HashSet<Point> CurrentVein { get; set; } = new();
		public Point? VeinCentroid = null;
		public int? CurrentVeinOre = null;

		public void ResetVein() {
			CurrentVein.Clear();
			CurrentVeinOre = null;
		}

		public void TraverseVein(Point point) {
			// Set up our validation lambda
			Func<Point, bool> validator = pnt => {
				if (CurrentVeinOre.HasValue) {
					Tile tCheck = Main.tile[pnt.X, pnt.Y];
					if (tCheck.HasTile && tCheck.TileType != CurrentVeinOre.Value) {
						return false;
					}
				}

				Tile t = Main.tile[pnt.X, pnt.Y];
				if (!t.HasTile || !Ores.Contains(t.TileType)) return false;
				return true;
			};

			// Create a BFSIterator that uses our validator
			var bfsIter = new BFSIterator(point, validator);

			// Operate on points from bfsIter
			foreach (Point pnt in bfsIter) {
				CurrentVein.Add(pnt);
				if (CurrentVeinOre == null) {
					Tile t = Main.tile[pnt.X, pnt.Y];
					CurrentVeinOre = t.TileType;
				}
			}
		}
		protected override bool IsValidTile(SmartCursorContext ctx, Point tile) {
			Tile t = Main.tile[tile.X, tile.Y];

			// Early exit: not an ore tile
			bool notAnOre = (!t.HasTile || !this.Ores.Contains(t.TileType));
			if (notAnOre) {
				return false;
			}

			bool isDifferentOre = (this.CurrentVeinOre.HasValue && this.CurrentVeinOre.Value != t.TileType);
			bool alreadyTrackingVein = (this.CurrentVein.Count > 0);
			if (isDifferentOre) {
				this.ResetVein();
			}

			if (!alreadyTrackingVein) {
				this.TraverseVein(tile);
			}

			bool tileInVein = this.CurrentVein.Contains(tile);
			bool hasLineOfSight = PlayerHasPath(ctx.Player.Center, t.TileType);

			if (tileInVein && hasLineOfSight) {
				// Force tile to be mineable
				ctx.RestrictedTiles.Remove(tile);
				return true;
			}

			return false;
		}

		private bool PlayerHasPath(Vector2 playerCenter, int targetTileType) {
			Point veinCentroid = GridUtils.GetCentroid(CurrentVein).ToPoint();
			Point playerCenterPoint = GridUtils.WorldToTile(playerCenter);

			var bresenham = new BresenhamIterator(playerCenterPoint, veinCentroid);
			foreach (Point pnt in bresenham) {
				Tile tile = Main.tile[pnt.X, pnt.Y];
				if (tile.HasTile && Main.tileSolid[tile.TileType] && tile.TileType != targetTileType) {
					return false;
				}
			}
			return true;
		}
		public static ApplianceHandle Register() {
			return Registry.RegisterAppliance(
				item => item.pick > 0,
				new VeinMinerAppliance(),
				SmartCursorRegistry.PRIORITY_NORMAL
			);
		}
	}
}
