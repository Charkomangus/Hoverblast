using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Tiles
{
    public class TilePath
    {

        public List<Tile> ListOfTiles = new List<Tile>();
        public int CostOfPath;
        public int NumberOfTiles;
        public Tile LastTile;
        

        /// <summary>
        /// Initialising an empty Tile Path
        /// </summary>
        public TilePath()
        {
        }

        /// <summary>
        /// Initialising a new Tile Path
        /// </summary>
        public TilePath(TilePath tilepath)
        {
            ListOfTiles = tilepath.ListOfTiles.ToList();
            CostOfPath = tilepath.CostOfPath;
            LastTile = tilepath.LastTile;
        }

        /// <summary>
        /// Add a tile to the list. Also increment the cost of tile and save the tile as the last tile added
        /// </summary>
        /// <param name="tile"></param>
        public void AddTile(Tile tile)
        {
            CostOfPath += tile.MovementCost;
            NumberOfTiles ++; 
            ListOfTiles.Add(tile);
            LastTile = tile;
        }

        /// <summary>
        /// Add a tile to the list. Also increment the cost of tile by one and save the tile as the last tile added
        /// </summary>
        /// <param name="tile"></param>
        public void AddStaticTile(Tile tile)
        {
            NumberOfTiles++;
            CostOfPath += 1;
            ListOfTiles.Add(tile);
            LastTile = tile;
        }
    }
}
    