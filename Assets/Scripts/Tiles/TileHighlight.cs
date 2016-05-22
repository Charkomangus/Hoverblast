using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    /// <summary>
    /// Handles the highlighting of tiles for movement and attacks
    /// </summary>
    public class TileHighlight
    {
        /// <summary>
        /// If static range and occupied and harddground is not given assume it's false, empty and true respectively
        /// </summary>
        /// <param name="originTile"></param>
        /// <param name="movementPoints"></param>
        /// <returns></returns>
        public static List<Tile> FindHighlight(Tile originTile, int movementPoints) 
        {
		return FindHighlight(originTile, movementPoints, new Vector2[0], false, false);
        }

        /// <summary>
        /// If occupied is not given assume it's occupied
        /// </summary>
        /// <param name="originTile"></param>
        /// <param name="movementPoints"></param>
        /// <param name="staticRange"></param>
        /// <param name="hardGround"></param>
        /// <returns></returns>
        public static List<Tile> FindHighlight(Tile originTile, int movementPoints, bool staticRange, bool hardGround)
        {
		return FindHighlight(originTile, movementPoints, new Vector2[0], staticRange, hardGround);
        }

        internal static object FindHighlight(Tile tile, int v1, Vector2[,] vector2, bool v2, bool v3)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  If static range and Hardground is not given assume it's false and true respectively
        /// </summary>
        /// <param name="originTile"></param>
        /// <param name="movementPoints"></param>
        /// <param name="occupied"></param>
        /// <returns></returns>
        public static List<Tile> FindHighlight(Tile originTile, int movementPoints, Vector2[] occupied) 
        {
            return FindHighlight(originTile, movementPoints, occupied, false, false);
        }
         
        /// <summary>
        /// This is the function the three above will reach. It makes a list with all the tiles that need to be highlighted
        /// </summary>
        /// <param name="originTile"></param>
        /// <param name="movementPoints"></param>
        /// <param name="occupied"></param>
        /// <param name="staticRange"></param>
        /// <param name="hardGround"></param> Take into account the cost of the ground.
        /// <returns></returns>
        public static List<Tile> FindHighlight(Tile originTile, int movementPoints, Vector2[] occupied, bool staticRange, bool hardGround)
        {
            var closed = new List<Tile>();
            var open = new List<TilePath>();
            var originPath = new TilePath();
           
            if (staticRange) 
                originPath.AddStaticTile(originTile);
            else 
                originPath.AddTile(originTile);

            open.Add(originPath);
            
            while (open.Count > 0)
            {
                var current = open[0];
                open.Remove(open[0]);
                
               
                if (closed.Contains(current.LastTile))
                {
                    continue;
                }
               
                

                //Is the cost of the ground relevant to the highlight? If not set the cost for 1 per tiles
                if (hardGround)
                {
                    if (current.CostOfPath > movementPoints + 1)
                        continue;

                }
                else
                {
                    if (current.ListOfTiles.Count > movementPoints + 1)
                        continue;
                    
                }

                closed.Add(current.LastTile);

                foreach (var tile in current.LastTile.Neighbors)
                {
                    if (tile.IsBlocked() || occupied.Contains(tile.ReturnPosition())) continue;
                    var newTilePath = new TilePath(current);
                    if 
                        (staticRange) newTilePath.AddStaticTile(tile);
                    else  
                        newTilePath.AddTile(tile);
				open.Add(newTilePath);
                   
                    
                }
            }
            closed.Remove(originTile);
            closed.Distinct(); //<--- Necessery? Find out.
            return closed;
        }


      

    }
}