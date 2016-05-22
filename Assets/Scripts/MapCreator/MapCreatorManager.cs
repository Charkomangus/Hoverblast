using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MapCreator
{
    public class MapCreatorManager : MonoBehaviour {
         //Creating an Instance so scripts can access it's variables.
        public static MapCreatorManager Instance;
        public TileType PalletSelection = TileType.Normal;
        public int MapSize;


        private List<List<Tile>> _map = new List<List<Tile>>();
        private Transform _mapTransform;

        /// <summary>
        /// UI buttons
        /// </summary>
        public Button Level1Save;
        public Button Level2Save;
        public Button Level3Save;
        public Button Level1Load;
        public Button Level2Load;
        public Button Level3Load;
        public Button ClearMap;
        public Button Normal;
        public Button Difficult;
        public Button VeryDifficult;
        public Button Impassible;
        public Slider InputSize;
        public Text Size;

        // Use this for initialization
        private void Awake () {
            Instance = this;
            Size.text = MapSize.ToString();
            InputSize.value = MapSize;
            _mapTransform = transform.FindChild("Map");

            //Setting up Listeners for the UI elements
            InputSize.onValueChanged.AddListener(delegate { MapSize = (int)InputSize.value;GenerateBlankMap(MapSize); Size.text = MapSize.ToString(); });
            Level1Save.onClick.AddListener(delegate { SaveMapToXml(0); });
            Level2Save.onClick.AddListener(delegate { SaveMapToXml(1); });
            Level3Save.onClick.AddListener(delegate { SaveMapToXml(2); });
            Level1Load.onClick.AddListener(delegate { LoadMapFromXml(0); Size.text = MapSize.ToString(); });
            Level2Load.onClick.AddListener(delegate { LoadMapFromXml(1); Size.text = MapSize.ToString(); });
            Level3Load.onClick.AddListener(delegate { LoadMapFromXml(2); Size.text = MapSize.ToString(); });
            ClearMap.onClick.AddListener(delegate { GenerateBlankMap(MapSize); });
            Normal.onClick.AddListener(delegate { PalletSelection = TileType.Normal;});
            Difficult.onClick.AddListener(delegate { PalletSelection = TileType.Difficult; });
            VeryDifficult.onClick.AddListener(delegate { PalletSelection = TileType.VeryDifficult; });
            Impassible.onClick.AddListener(delegate { PalletSelection = TileType.Impassible; });
        }
        

        private void Start()
        {
            GenerateBlankMap(MapSize);
        }
        
        /// <summary>
        ///Create a completely blank Map
        /// </summary>
        /// <param name="mapSize"></param>
        private void GenerateBlankMap(int mapSize) {
            MapSize = mapSize;

            //Remove all children
            for(var i = 0; i < _mapTransform.childCount; i++) {
                Destroy (_mapTransform.GetChild(i).gameObject);
            }

            _map = new List<List<Tile>>();
            for (var x = 0; x < MapSize; x++)
            {
                var row = new List<Tile>();
                for (var y = 0; y < MapSize; y++)
                {
                    var tile =((GameObject)Instantiate(PrefabHolder.Instance.BaseTilePrefab,new Vector3(x - Mathf.Floor(MapSize/2f), 0, -y + Mathf.Floor(MapSize/2f)),Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                    tile.transform.parent = _mapTransform;
                    tile.SetPosition(new Vector2(x, y));
                    tile.SetType(TileType.Normal);
                    row.Add(tile);
                }
                _map.Add(row);
            }
        }
      
        /// <summary>
        /// Load a map from an XML file
        /// </summary>
        /// <param name="level"></param>
        private void LoadMapFromXml(int level) {
          
            //Load map level
            var container = MapSaveLoad.Load("level" + level + ".xml");
            MapSize = container.Size;

            //Remove all children
            for(var i = 0; i < _mapTransform.childCount; i++) {
                Destroy (_mapTransform.GetChild(i).gameObject);
            }

            _map = new List<List<Tile>>();
            for (var x = 0; x < MapSize; x++) 
            {
                var row = new List<Tile>();
                for (var y = 0; y < MapSize; y++) 
                {
                    var tile = ((GameObject)Instantiate(PrefabHolder.Instance.BaseTilePrefab, new Vector3(x - Mathf.Floor(MapSize/2f),0, -y + Mathf.Floor(MapSize/2f)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                    tile.transform.parent = _mapTransform;
                    tile.SetPosition(new Vector2(x, y));
                    tile.SetType((TileType)container.Tiles.First(position => position.LocX == x && position.LocY == y).Id);
                    row.Add (tile);
                }
                _map.Add(row);
            }
        }


        /// <summary>
        /// Save current map to an xml file
        /// </summary>
        /// <param name="level"></param>
        private void SaveMapToXml(int level)
        {
            MapSaveLoad.Save(MapSaveLoad.CreateMapContainer(_map), "level" + level +".xml");
        }
    }
}
