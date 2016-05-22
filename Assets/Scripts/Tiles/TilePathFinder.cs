using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    public class TilePathFinder : MonoBehaviour {


        /// <summary>
        /// If occupied is not given assume it's occupied
        /// </summary>
        /// <param name="originTile"></param>
        /// <param name="destinationTile"></param>
        /// <returns></returns>
        public static List<Tile> FindPath(Tile originTile, Tile destinationTile)
        {
            return FindPath(originTile, destinationTile, new Vector2[0]);
        }

        /// <summary>
        ///  Make a path from the origin to the chosen Tile
        /// </summary>
        /// <param name="originTile"></param>
        /// <param name="destinationTile"></param>
        /// <param name="occupied"></param>
        /// <returns></returns>
        public static List<Tile> FindPath(Tile originTile, Tile destinationTile, Vector2[] occupied)
        {
            var closed = new List<Tile>();
            var open = new List<TilePath>();
            var originPath = new TilePath();
            originPath.AddTile(originTile);
            open.Add(originPath);
		
            while (open.Count > 0) 
            {
                var current = open[0];
                open.Remove(open[0]);
			
                if (closed.Contains(current.LastTile)) continue; 
                
                if (current.LastTile == destinationTile) 
                {
                    current.ListOfTiles.Remove (originTile);
                    return current.ListOfTiles;
                }
                closed.Add(current.LastTile);

                foreach (var tile in current.LastTile.ReturnNeighbors())
                {
                    if (tile.Blocked) continue;
                    var newTilePath = new TilePath(current);
                    newTilePath.AddTile(tile);
                    open.Add(newTilePath);
                }
            }
            return null;
        }
    }
}
