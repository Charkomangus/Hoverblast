using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.MapCreator
{
    /// <summary>
    /// This class generates the map. 
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        public GameObject TileList;
        private int _mapSize;
        private List<List<Tile>> _map = new List<List<Tile>>();
        /// <summary>
        /// Populate the map with tiles
        /// </summary>
        /// <param name="mapSize"></param> Given from Game manager
        /// <param name="tilePrefab"></param> Given from Game manager
        public void GenerateMap(int mapSize, GameObject tilePrefab)
        {
            for (var i = 0; i < mapSize; i++)
            {
                var row = new List<Tile>();
                for (var j = 0; j < mapSize; j++)
                {
                    var tile = ((GameObject)Instantiate(tilePrefab, new Vector3(i - Mathf.Floor(mapSize / 2f), 0, -j + Mathf.Floor(mapSize / 2f)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                    tile.transform.SetParent(TileList.transform);
                    tile.SetPosition(new Vector3(i, j, 0));
                    row.Add(tile);
                }
                _map.Add(row);
            }
        }


        /// <summary>
        /// Use an xml file to create a map
        /// </summary>
        /// <param name="mapTransform"></param>
        /// <param name="level"></param>
        public void LoadMapFromXml(Transform mapTransform, int level)
        {
            var container = MapSaveLoad.LoadFromResources("level" + level);
           


            _mapSize = container.Size;

            //Initially remove all children
            for (var i = 0; i < mapTransform.childCount; i++)
            {
                Destroy(mapTransform.GetChild(i).gameObject);
            }

            _map = new List<List<Tile>>();
            for (var i = 0; i < _mapSize; i++)
            {
                var row = new List<Tile>();
                for (var j = 0; j < _mapSize; j++)
                {
                    var tile = ((GameObject)Instantiate(PrefabHolder.Instance.BaseTilePrefab, new Vector3(i - Mathf.Floor(_mapSize / 2f), 0, -j + Mathf.Floor(_mapSize / 2f)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                    tile.transform.parent = mapTransform;
                    tile.SetPosition(new Vector2(i, j)); 
                    tile.SetType((TileType)container.Tiles.First(x => x.LocX == i && x.LocY == j).Id);
                    row.Add(tile);
                }
                _map.Add(row);
            }
        }
        #region Sets & Returns
        /// <summary>
        /// Return the currently generated Map
        /// </summary>
        /// <returns></returns>
        public List<List<Tile>> ReturnMap()
        {
            return _map;
        }

        /// <summary>
        /// Return the currently generated Maps size
        /// </summary>
        /// <returns></returns>
        public int ReturnMapSize()
        {
            return _mapSize;
        }
        #endregion
    }
}
