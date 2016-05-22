using System.Collections.Generic;
using Assets.Scripts.MapCreator;
using Assets.Scripts.MAIN_MANAGERS;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Tiles
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {

        public Vector2 GridPosition = Vector2.zero;

        private GameObject _prefab;
        private GameObject _renderType;
        public TileType Type = TileType.Normal;
        public int MovementCost = 1;
        public bool Blocked;
        public List<Tile> Neighbors = new List<Tile>();
        private List<List<Tile>> _map;
        private int _mapSize;

        // Use this for initialization
        private void Start()
        {

            if (SceneManager.GetActiveScene().name == "Game")
            {
                _map = BattleManager.Instance.ReturnMap();
                _mapSize = BattleManager.Instance.ReturnMapSize();
                GenerateNeighbors();
            }
        }

        /// <summary>
        /// Each tile will find out what all it's neighboring tiles are and store them in a list. 
        /// </summary>
        public void GenerateNeighbors()
        {
            Neighbors = new List<Tile>();

            //NORTH
            if (GridPosition.y > 0)
            {
                var n = new Vector2(GridPosition.x, GridPosition.y - 1);
                Neighbors.Add(_map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
            }

            //WEST
            if (GridPosition.x > 0)
            {
                var n = new Vector2(GridPosition.x - 1, GridPosition.y);
                Neighbors.Add(_map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
            }
            //EAST
            if (GridPosition.x < _mapSize - 1)
            {
                var n = new Vector2(GridPosition.x + 1, GridPosition.y);
                Neighbors.Add(_map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
            }
            ////NORTHWEST
            if (GridPosition.y > 0 && GridPosition.x > 0)
            {
                var n = new Vector2(GridPosition.x - 1, GridPosition.y - 1);
                Neighbors.Add(_map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
            }
            //NORTHEAST
            if (GridPosition.y > 0 && GridPosition.x < _mapSize - 1)
            {
                var n = new Vector2(GridPosition.x + 1, GridPosition.y - 1);
                Neighbors.Add(_map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
            }
            //SOUTHWEST
            if (GridPosition.y < _mapSize - 1 && GridPosition.x > 0)
            {
                var n = new Vector2(GridPosition.x - 1, GridPosition.y + 1);
                Neighbors.Add(_map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
            }
            //SOUTHEAST
            if (GridPosition.y < _mapSize - 1 && GridPosition.x < _mapSize - 1)
            {
                var n = new Vector2(GridPosition.x + 1, GridPosition.y + 1);
                Neighbors.Add(_map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
            }
            //SOUTH
            if (GridPosition.y < _mapSize - 1)
            {
                var n = new Vector2(GridPosition.x, GridPosition.y + 1);
                Neighbors.Add(_map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
            }
        }

        // Update is called once per frame
        private void Update()
        {

        }



        public void SetType(TileType type)
        {
            Type = type;
            //definitions of TileType properties
            switch (type)
            {
                case TileType.Normal:
                    MovementCost = 1;
                    Blocked = false;
                    _prefab = PrefabHolder.Instance.TileNormalPrefab;
                    break;

                case TileType.Difficult:
                    MovementCost = 2;
                    Blocked = false;
                    _prefab = PrefabHolder.Instance.TileDifficultPrefab;
                    break;

                case TileType.VeryDifficult:
                    MovementCost = 3;
                    Blocked = false;
                    _prefab = PrefabHolder.Instance.TileVeryDifficultPrefab;
                    break;

                case TileType.Impassible:
                    MovementCost = 9999;
                    Blocked = true;
                    _prefab = PrefabHolder.Instance.TileImpassiblePrefab;
                    break;

                default:
                    MovementCost = 1;
                    Blocked = false;
                    _prefab = PrefabHolder.Instance.TileNormalPrefab;
                    break;
            }

            GenerateVisuals();
        }

        public void GenerateVisuals()
        {
            var container = transform.FindChild("Visuals").gameObject;
            //initially remove all children
            for (var i = 0; i < container.transform.childCount; i++)
            {
                Destroy(container.transform.GetChild(i).gameObject);
            }

            var renderType =
                (GameObject) Instantiate(_prefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            renderType.transform.parent = container.transform;

            _renderType = renderType;
        }


        private void OnMouseEnter()
        {
            if (SceneManager.GetActiveScene().name != "MapCreatorScene") return;
            if (Input.GetMouseButton(0))
            {
                SetType(MapCreatorManager.Instance.PalletSelection);
            }
            else if (Input.GetMouseButton(1))
            {
                SetType(TileType.Normal);

            }
        }

        /// <summary>
        /// When tile is clciked....
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {

            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    switch (eventData.button)
                    {
                        case PointerEventData.InputButton.Left:
                            UseTile(this);
                            break;
                        case PointerEventData.InputButton.Middle:
                            BattleManager.Instance.RemoveTileHighlights();
                            BattleManager.Instance.ReturnCurrentPlayer().SetMode("Idle");
                            GameManager.Instance.GameUi.GetComponent<UiManager>().CloseInfoBox();
                            break;
                        case PointerEventData.InputButton.Right:
                           GameManager.Instance.GameUi.GetComponent<UiManager>().OpenInfoBox(this);
                            break;
                        default:
                            Debug.Log("Invalid Input on Tile");
                            break;
                    }
                    break;
                case "MapCreatorScene":
                    switch (eventData.button)
                    {
                        case PointerEventData.InputButton.Left:
                            SetType(MapCreatorManager.Instance.PalletSelection);
                            break;
                        case PointerEventData.InputButton.Middle:
                             Debug.Log("Invalid Input");
                            break;
                        case PointerEventData.InputButton.Right:
                            SetType(TileType.Normal);
                            break;
                        default:
                            Debug.Log("Invalid Input");
                            break;
                    }
                    break;
                   
            }
        }



        public void UseTile(Tile tile)
        {
            switch (BattleManager.Instance.ReturnCurrentPlayer().ReturnMode())
            {

                case "Move":
                    BattleManager.Instance.MovePlayer(tile);
                    break;
                case "Attack":
                    BattleManager.Instance.AttackPlayer(tile);
                    break;
                case "Teleport":
                    BattleManager.Instance.TeleportPlayer(tile);
                    break;
                case "Force Field":
                    BattleManager.Instance.OpenBarrier(tile);
                    break;
                case "Scope":
                    BattleManager.Instance.ScopedAttack(tile);
                    break;
                case "Medpack":
                    BattleManager.Instance.Medpack(tile);
                    break;
                default:
                    Debug.Log("Something has really got wrong");
                    break;
            }
        }

        #region Sets & Returns

        /// <summary>
        /// Set the Tiles render Type
        /// </summary>
        /// <param name="renderType"></param>
        public void SetRenderType(GameObject renderType)
        {
            _renderType = renderType;
        }

        /// <summary>
        /// Return The tiles render Type
        /// </summary>
        /// <returns></returns>
        public GameObject ReturnRenderType()
        {
            return _renderType;
        }



        /// <summary>
        /// Set the Tiles Position
        /// </summary>
        /// <param name="newPosition"></param>
        public void SetPosition(Vector2 newPosition)
        {
            GridPosition = newPosition;
        }

        /// <summary>
        /// Return The movement cost of this particular tile
        /// </summary>
        /// <returns></returns>
        public int ReturnCost()
        {
            return MovementCost;
        }

        /// <summary>
        /// Return the Tiles neighbors
        /// </summary>
        /// <returns></returns>
        public List<Tile> ReturnNeighbors()
        {
            return Neighbors;
        }

        /// <summary>
        /// Return the current player status
        /// </summary>
        /// <returns></returns>
        public Vector2 ReturnPosition()
        {
            return GridPosition;
        }

        /// <summary>
        /// Returns the tiles type
        /// </summary>
        /// <returns></returns>
        public TileType ReturnType()
        {
            return Type;
        }

        /// <summary>
        /// Returns true if the tile is impassable
        /// </summary>
        /// <returns></returns>
        public bool IsBlocked()
        {
            return Blocked;
        }
        #endregion
    }
}
