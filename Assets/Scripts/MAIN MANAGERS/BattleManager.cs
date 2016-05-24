using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CameraInput;
using Assets.Scripts.MapCreator;
using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using Assets.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.MAIN_MANAGERS
{
    public enum BattleState
    {
        Battle,
        ResolutionPlayerOne,
        ResolutionPlayerTwo,
        Win,
        Loss
    }

    public enum DangerLevel
    {
        Tension,
        Fight,
        Danger
    }

    /// <summary>
    /// This is the main classe of the project. 
    /// It is used as a bridge between scripts and to assign variables to them.
    ///  </summary>
    public class BattleManager : MonoBehaviour
    {
        //Creating an Instance so scripts can access it's variables.
        public static BattleManager Instance;



        //Importing Camera
        public CameraController MainCamera;

        //Importing all necessery variables
        public int MapSize;
        public int TeamSize;
        public Transform MapTransform;
        private List<Player> _healingTargets;


        //Importing all necessery prefabs
        public GameObject TilePrefab;
        public GameObject Barrier;
        public List<GameObject> Barriers;
        private GameObject _barrier;

        //Importing all necessery componments
        public List<Player> Players;
        public int CurrentPlayerIndex;
        private List<List<Tile>> _map;

        //Importing Sound
        public AudioClip LaserTurretAudio;
        public AudioClip SRIUSAudio;
        public AudioClip BeamCannonAudio;
        public AudioClip SniperRifleAudio;
        public AudioClip PlasmaSpearAudio;
        public AudioClip TaserAudio;
        public AudioClip TeleportationAudio;
        public AudioClip BarrierAudio;
        public AudioClip MedpackAudio;

        //Battle State & DangerLevel
        public BattleState _battlestate;
        public DangerLevel _dangerLevel;
        private bool _pvp;

        private void Awake()
        {
          
            Instance = this;
            MapTransform = transform.FindChild("Map");
            Instance.GetComponent<MapGenerator>().LoadMapFromXml(Instance.MapTransform, GameManager.Instance.ReturnLevel());
            _map = GetComponent<MapGenerator>().ReturnMap(); //Save existing map
            MapSize = GetComponent<MapGenerator>().ReturnMapSize(); //Save existing mapsize
            Players = GetComponent<PlayerGenerator>().ReturnPlayers(); //Save existing players
            _barrier = new GameObject();
        }

        // Use this for initialization
        private void Start()
        {
            _healingTargets = new List<Player>();
            

        }

        /// <summary>
        /// Pickup mapsize and continue on
        /// </summary>
        public void GeneratePlayers(bool mode, GameObject aiTank, GameObject aiJet, GameObject player1Tank,
            GameObject player1Jet, GameObject player2Tank, GameObject player2Jet, List<CreatedCharacter> characterList, List<Player> Originalplayers, bool IsLoaded)
        {
            _pvp = mode;
            GetComponent<PlayerGenerator>()
                .GeneratePlayers(mode, MapSize, aiTank, aiJet, player1Tank, player1Jet, player2Tank, player2Jet,
                    characterList, Originalplayers, IsLoaded);
        }



        // Update is called once per frame
        private void Update()
        {

            if (_battlestate == BattleState.Battle)
            {
                //If a unit is dead skip it's turn
                if (Players[CurrentPlayerIndex].IsAlive())
                {
                   
                    Players[CurrentPlayerIndex].TurnUpdate();

                }
                else
                {
                    NextTurn();
                }

                //Remove Tile Highlights if player is in idle mode.
                if (Players[CurrentPlayerIndex].ReturnMode() == "Idle")
                    RemoveTileHighlights();
            }
            DetermineBattleState();
            DetermineDangerLevel();


        }

        /// <summary>
        /// Returns how many memebers of a team are left alive
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public int DetermineAliveInTeam(int team)
        {
            return Players.Where(player => player.ReturnTeam() == team).Count(player => player.Alive);
        }

        /// <summary>
        /// Determine the danger level in order to set the apropriate theme
        /// </summary>
        public void DetermineDangerLevel()
        {
            if (DetermineAliveInTeam(0) <= 2 || DetermineAliveInTeam(1) <= 2)
            {
                _dangerLevel = DangerLevel.Danger;
            }
            else
            {
                foreach (var player in Players.Where(player => player.ReturnHp() != player.ReturnMaxHp()))
                {
                    _dangerLevel = DangerLevel.Fight;
                }
            }
        }

        /// <summary>
        /// End current turn and start a new one.
        /// </summary>
        public void NextTurn()
        {
            CurrentPlayerIndex = GetComponent<TurnManager>().NextTurn(CurrentPlayerIndex, Players);
            MainCamera.SetTarget(Players[CurrentPlayerIndex].transform);
        }


   



        /// <summary>
        /// 
        /// </summary>
        public void DetermineBattleState()
        {
            if (_pvp)
            {
                if (DetermineAliveInTeam(0) > 0 && DetermineAliveInTeam(1) > 0)
                    _battlestate = BattleState.Battle;
                else if (DetermineAliveInTeam(0) <= 0)
                    _battlestate = BattleState.ResolutionPlayerOne;
                else if (DetermineAliveInTeam(1) <= 0)
                    _battlestate = BattleState.ResolutionPlayerTwo;
            }
            else
            {
                if (DetermineAliveInTeam(0) > 0 && DetermineAliveInTeam(1) > 0)
                    _battlestate = BattleState.Battle;
                else if (DetermineAliveInTeam(0) <= 0)
                    _battlestate = BattleState.Loss;
                else if (DetermineAliveInTeam(1) <= 0)
                    _battlestate = BattleState.Win;
            }


        }

        #region Highlight

        //If Ignore Players is not given assume it's true
        public void HighlightTiles(Vector2 originLocation, Color highlightColor, int distance, bool hardground)
        {
            HighlightTiles(originLocation, highlightColor, distance, true, false, hardground);
        }

        //If Ignore players is set to true just highlight every tile in the distance given
        //Else highlight everything but the players position.
        public void HighlightTiles(Vector2 originLocation, Color highlightColor, int distance, bool ignorePlayers, bool self, bool hardground)
        {
            //Check if ignore Players is activated. If it's true then highlight tiles to the given distance.
            //If it's false the do the same excluding the tiles that have players on them


            var highlightedTiles = ignorePlayers
                ? TileHighlight.FindHighlight(_map[(int) originLocation.x][(int) originLocation.y], distance, highlightColor == Color.red, hardground)
                : TileHighlight.FindHighlight(_map[(int) originLocation.x][(int) originLocation.y], distance, Players.Where(x => x.ReturnPosition() != originLocation).Select(x => x.ReturnPosition()).ToArray(),
                    highlightColor == new Color(255, 0, 0, 0.7f), true);

            //Highlight origin tile
            if (self)
            {
                highlightedTiles.Add(_map[(int) originLocation.x][(int) originLocation.y]);
            }
            foreach (var tile in highlightedTiles)
            {
                tile.ReturnRenderType().transform.GetComponent<Renderer>().materials[0].color = highlightColor;
            }



        }


        /// <summary>
        /// Remove any Highlights placed on the tiles
        /// </summary>
        public void RemoveTileHighlights()
        {
            for (var x = 0; x < MapSize; x++)
                for (var y = 0; y < MapSize; y++)
                    if (!_map[x][y].Blocked)
                        _map[x][y].ReturnRenderType().transform.GetComponent<Renderer>().materials[0].color =
                            Color.white;
        }

        #endregion

        

        #region Unit Movement


        /// <summary>
        /// Change unit destination to tile selected
        /// </summary>
        /// <param name="destTile"></param>
        public void MovePlayer(Tile destTile)
        {
            if (destTile.ReturnRenderType().transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.IsBlocked())
            {
                RemoveTileHighlights();
                Players[CurrentPlayerIndex].SetMode("Idle");
                foreach (var tile in TilePathFinder.FindPath(_map[(int) Players[CurrentPlayerIndex].ReturnPosition().x][(int) Players[CurrentPlayerIndex].ReturnPosition().y], destTile,
                            Players.Where(x =>x.ReturnPosition() != destTile.ReturnPosition() && x.ReturnPosition() != Players[CurrentPlayerIndex].ReturnPosition())
                                .Select(x => x.ReturnPosition()).ToArray()))
                {
                    Players[CurrentPlayerIndex].PositionQueue.Add(_map[(int) tile.ReturnPosition().x][(int) tile.ReturnPosition().y].transform.position + 1.2f*Vector3.up);
                }
                Players[CurrentPlayerIndex].SetPosition(destTile.ReturnPosition());

            }
            else
                Debug.Log("destination invalid");

        }
      

        /// <summary>
        /// Attack with current unit
        /// </summary>
        /// <param name="destTile"></param>
        public void AttackPlayer(Tile destTile)
        {

            if (destTile.ReturnRenderType().transform.GetComponent<Renderer>().materials[0].color != Color.white &&      !destTile.IsBlocked())
            {
                var attacker = Players[CurrentPlayerIndex];
                Player target = null;
                //Find Target
                foreach (var player in Players.Where(player => player.ReturnPosition() == destTile.ReturnPosition() && player.IsAlive() && player.ReturnTeam() != attacker.ReturnTeam()))
                    target = player;

                if (target == null) return;

                //Rotation
                attacker.Rotation(1, destTile);
                target.Rotation(1, attacker.ReturnCurrentTile());

                RemoveTileHighlights();

                //Play apropriate sound & effect
                attacker.PlaySoundEffect(SetSound(attacker));
                attacker.GetComponent<ShootingManager>().ShootNow(attacker, target);
                attacker.SetActionPoints(attacker.ReturnActionPoints() - attacker.ReturnActionCost());
                attacker.SetMode("Idle");

                //Cover
                float coverbonus = 0;
                if (target.ReturnCurrentTile().Neighbors.Any(tile => tile.ReturnType() == TileType.Impassible))
                    coverbonus = 0.01f;

                //attack logic
                var hit = Random.Range(0.0f, 1.0f) <= attacker.ReturnAttackChance() - (target.ReturnEvade() + coverbonus);
                //roll to hit
                if (hit)
                {
                    //Determine ther amount damage to do. If it's less than zero do zero damage.
                    var damage = attacker.ReturnWeapon().WeaponName == "Plasma Spear" ? Mathf.Max(0, (int)Mathf.Floor(attacker.ReturnDamage() + Random.Range(0, attacker.ReturnDamageRoll() - target.ReturnDamageReduction() / 2f))) : Mathf.Max(0, (int)Mathf.Floor(attacker.ReturnDamage() + Random.Range(0, attacker.ReturnDamageRoll() - target.ReturnDamageReduction())));



                    //Check if the target has a barrier
                    if (target.ReturnBarrierStatus())
                    {
                        target.SetBarrierHealth(target.ReturnBarrierHealth() - damage);
                        UiManager.Instance.OpenHitBox("BARRIER!");
                    }

                    else
                    {

                        target.SetHp(target.ReturnHp() - damage);
                        if (target.ReturnHp() <= 0)
                        {
                            target.ReturnHealthBar().TriggetHitCounter("DEAD!");
                            UiManager.Instance.OpenHitBox("DEAD!");
                        }
                        else
                        {
                            target.ReturnHealthBar().TriggetHitCounter(damage + " DAMAGE!");
                            UiManager.Instance.OpenHitBox(damage + " DAMAGE!");
                        }
                    }
                    Debug.Log(attacker.ReturnName() + " successfuly hit " + target.ReturnName() + " for " + damage +
                              " damage!");
                    Players[CurrentPlayerIndex].SetMode("Idle");
                }
                else
                {
                    Debug.Log(attacker.ReturnName() + " missed " + target.ReturnName() + "!");
                    Players[CurrentPlayerIndex].SetMode("Idle");
                    UiManager.Instance.OpenHitBox("MISSED!");
                    target.ReturnHealthBar().TriggetHitCounter("MISSED!");
                }
            }
            else
            {
                if (IsUser())
                    Debug.Log("Destination is invalid(Attack)");
                else
                {
                    Players[CurrentPlayerIndex].SetMode("Idle");
                    Players[CurrentPlayerIndex].SetActionPoints(0);
                }
                    
                
            }
        }

        #endregion
#region Special Abilities
        /// <summary>
        /// Attack with current unit using the scoped abilityz
        /// </summary>
        /// <param name="destTile"></param>
        public void ScopedAttack(Tile destTile)
        {
            if (destTile.ReturnRenderType().transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.IsBlocked())
            {
                var attacker = Players[CurrentPlayerIndex];
                Player target = null;
                foreach (var player in Players.Where(player => player.ReturnPosition() == destTile.ReturnPosition() && player.IsAlive() && player.ReturnTeam() != attacker.ReturnTeam()))
                    target = player;

                if (target == null) return;

                Players[CurrentPlayerIndex].SetCooldown(3);
                RemoveTileHighlights();

               //Rotation
                attacker.Rotation(1, destTile);
                target.Rotation(1, attacker.ReturnCurrentTile());

                RemoveTileHighlights();

                //Play apropriate sound & effect
                attacker.PlaySoundEffect(SetSound(attacker));
                attacker.GetComponent<ShootingManager>().ShootNow(attacker, target);



                attacker.SetActionPoints(attacker.ReturnActionPoints() - 1);
                attacker.SetMode("Idle");

                //Cover
                float coverbonus = 0;
                if (target.ReturnCurrentTile().Neighbors.Any(tile => tile.ReturnType() == TileType.Impassible))
                    coverbonus = 0.01f;
               
                
                //attack logic
                var hit = Random.Range(0.0f, 1.0f) <= attacker.ReturnAttackChance()+0.10f - (target.ReturnEvade() + coverbonus); //roll to hit
               
                if (hit)
                {
                    int penalty;
                    if (attacker.ReturnWeapon().WeaponName == "Plasma Spear" ||
                        (attacker.ReturnWeapon().WeaponName == "Shock Taser"))
                    {
                        penalty = 5;
                    }
                    else
                    {
                        penalty = 0;
                    }

                    //Determine ther amount damage to do. If it's less than zero do zero damage.
                    int damage;
                    if (attacker.ReturnWeapon().WeaponName == "Plasma Spear") // Armor pierce
                    {
                         damage =Mathf.Max(0,(int)Mathf.Floor(attacker.ReturnDamage() +Random.Range(0,attacker.ReturnDamageRoll() - target.ReturnDamageReduction()/2f))) -
                            penalty + 2;
                    }
                    else
                    {
                         damage =Mathf.Max(0,(int)Mathf.Floor(attacker.ReturnDamage() +Random.Range(0,attacker.ReturnDamageRoll() - target.ReturnDamageReduction()))) -
                            penalty + 2;
                    }
                    //Check if the target has a barrier
                    if (target.ReturnBarrierStatus())
                    {
                        target.SetBarrierHealth(target.ReturnBarrierHealth() - damage);
                        UiManager.Instance.OpenHitBox("BARRIER!");
                    }
                 
                    else
                    {
                       
                        target.SetHp(target.ReturnHp() - damage);
                        if (target.ReturnHp() <= 0)
                        {
                            target.ReturnHealthBar().TriggetHitCounter("DEAD!");
                            UiManager.Instance.OpenHitBox("DEAD!");
                        }
                        else
                        {
                            target.ReturnHealthBar().TriggetHitCounter(damage + " DAMAGE!");
                            UiManager.Instance.OpenHitBox(damage + " DAMAGE!");
                        }
                    }
                    Debug.Log(attacker.ReturnName() + " successfuly hit " + target.ReturnName() + " for " + damage + " damage!");
                    Players[CurrentPlayerIndex].SetMode("Idle");
                }
                else
                {
                    Debug.Log(attacker.ReturnName() + " missed " + target.ReturnName() + "!");
                    Players[CurrentPlayerIndex].SetMode("Idle");
                    UiManager.Instance.OpenHitBox("MISSED!");
                    target.ReturnHealthBar().TriggetHitCounter("MISSED!");
                }
            }
            else
               if (IsUser())
                Debug.Log("Destination is invalid(Attack)");
            else
            {
                Players[CurrentPlayerIndex].SetMode("Idle");
                Players[CurrentPlayerIndex].SetActionPoints(0);
            }

        }

        /// <summary>
        /// Attack with current unit using the scoped abilityz
        /// </summary>
        /// <param name="destTile"></param>
        public void Medpack(Tile destTile)
        {

            _healingTargets.RemoveRange(0, _healingTargets.Count);

            if (destTile.ReturnRenderType().transform.GetComponent<Renderer>().materials[0].color != Color.white &&
                !destTile.IsBlocked())
            {
                {
                    RemoveTileHighlights();
                    //Action Point Cost
                    Players[CurrentPlayerIndex].SetActionPoints(Players[CurrentPlayerIndex].ReturnActionPoints() - 1);
                    Players[CurrentPlayerIndex].SetCooldown(3);
                    Players[CurrentPlayerIndex].SetHp(Players[CurrentPlayerIndex].ReturnHp() + 30);
                    // FInd all the players around this unit  and put them in a list
                    foreach (var player in from tiles in destTile.ReturnNeighbors()
                        from player in Players
                        where player.ReturnPosition() == tiles.ReturnPosition() &&
                              player.ReturnTeam() == Players[CurrentPlayerIndex].ReturnTeam()
                        select player)
                    {
                        _healingTargets.Add(player);
                    }
                    //Play apropriate sound
                    Players[CurrentPlayerIndex].PlaySoundEffect(SetSound(Players[CurrentPlayerIndex]));
                    

                }
                foreach (var player in _healingTargets)
                    player.SetHp(player.ReturnHp() + 20);
            }
            else if (IsUser())
                Debug.Log("Destination is invalid(Attack)");
            else
            {
                Players[CurrentPlayerIndex].SetMode("Idle");
                Players[CurrentPlayerIndex].SetActionPoints(0);
            }



        }




    
        /// <summary>
        /// Change unit destination to tile selected
        /// </summary>
        /// <param name="destTile"></param>
        public void OpenBarrier(Tile destTile)
        {

            if (destTile.ReturnRenderType().transform.GetComponent<Renderer>().material.color != Color.white && !destTile.Blocked)
            {
                RemoveTileHighlights();
                Players[CurrentPlayerIndex].SetMode("Force Field");
                Players[CurrentPlayerIndex].SetCooldown(3);
                _barrier = null;
                foreach (var child in Players[CurrentPlayerIndex].transform.Cast<Transform>().Where(child => child.tag == "Barrier"))
                {
                    Barriers.Add(_barrier);
                    _barrier = child.gameObject;
                }

                if (_barrier == null)
                {
                    _barrier = (GameObject)Instantiate(Barrier, Players[CurrentPlayerIndex].transform.position, Players[CurrentPlayerIndex].transform.rotation);
                    _barrier.transform.SetParent(Players[CurrentPlayerIndex].transform);
                    Players[CurrentPlayerIndex].SetBarrierHealth(20);
                    _barrier.GetComponent<Animator>().SetBool("IsOpen", true);
                   

                }
                else
                {
                    Players[CurrentPlayerIndex].SetBarrierHealth(20);
                    _barrier.GetComponent<Animator>().SetBool("IsOpen", true);
                }
                //Play apropriate sound
                Players[CurrentPlayerIndex].SetActionPoints(Players[CurrentPlayerIndex].ReturnActionPoints() - 1);
                Players[CurrentPlayerIndex].PlaySoundEffect(SetSound(Players[CurrentPlayerIndex]));
                
            }
            else if (IsUser())
                Debug.Log("Destination is invalid(Attack)");
            else
            {
                Players[CurrentPlayerIndex].SetMode("Idle");
                Players[CurrentPlayerIndex].SetActionPoints(0);
            }

        }




        /// <summary>
        /// Change unit destination to tile selected
        /// </summary>
        /// <param name="destTile"></param>
        public void TeleportPlayer(Tile destTile)
        {
            if (destTile.ReturnRenderType().transform.GetComponent<Renderer>().material.color != Color.white && !destTile.Blocked)
            {
                RemoveTileHighlights();
                Players[CurrentPlayerIndex].SetMode("Teleport");
                //Play apropriate sound
                Players[CurrentPlayerIndex].PlaySoundEffect(SetSound(Players[CurrentPlayerIndex]));
                MainCamera.SetTarget(Players[CurrentPlayerIndex].transform);

                Players[CurrentPlayerIndex].SetCooldown(3);
                //Set Rotation
                var targetRotation = Quaternion.LookRotation(destTile.transform.position - Players[CurrentPlayerIndex].transform.position);
                Players[CurrentPlayerIndex].transform.rotation = Quaternion.Slerp(Players[CurrentPlayerIndex].transform.rotation, targetRotation, 120 * Time.deltaTime);
                Players[CurrentPlayerIndex].transform.eulerAngles = new Vector3(0, Players[CurrentPlayerIndex].transform.eulerAngles.y, 0);
                //Set Position
                Players[CurrentPlayerIndex].transform.position = destTile.transform.position;
                Players[CurrentPlayerIndex].SetPosition(destTile.ReturnPosition());
                Players[CurrentPlayerIndex].transform.position = new Vector3(Players[CurrentPlayerIndex].transform.position.x, 2.4f, Players[CurrentPlayerIndex].transform.position.z);
                Players[CurrentPlayerIndex].SetActionPoints(Players[CurrentPlayerIndex].ReturnActionPoints() - 1);

            }
            else if (IsUser())
                Debug.Log("Destination is invalid(Attack)");
            else
            {
                Players[CurrentPlayerIndex].SetMode("Idle");
                Players[CurrentPlayerIndex].SetActionPoints(0);
            }

        }
        #endregion




        #region Sets & Returns

        /// <summary>
        /// Change sound effect to the apropriate one
        /// </summary>
        public AudioClip SetSound(Player player)
        {
            var audio = new AudioClip();
            switch (player.ReturnMode())
            {
                case "Attack":
                case "Scope":
                    switch (player.ReturnWeapon().WeaponName)
                    {
                        case "Laser Turret":
                            audio = LaserTurretAudio;
                            break;
                        case "Beam Cannon":
                            audio = BeamCannonAudio;
                            break;
                        case "Sniper Rifle":
                            audio = SniperRifleAudio;
                            break;
                        case "SRiuS":
                            audio = SRIUSAudio;
                            break;
                        case "Plasma Spear":
                            audio = PlasmaSpearAudio;
                            break;
                        case "Shock Taser":
                            audio = TaserAudio;
                            break;
                        default:
                            audio = LaserTurretAudio;
                            break;
                    }
                    break;
                case "Teleport":
                    audio = TeleportationAudio;
                    break;
                case "Medpack":
                    audio = MedpackAudio;
                    break;
                case "Force Field":
                    audio = BarrierAudio;
                    break;
            }

            return audio;
        }

        /// <summary>
        /// Change shooting effect to the apropriate one
        /// </summary>
        public void SetShoot(Player player)
        {
        //   player.ReturnShootingManager().SetEverything();
        }

        /// <summary>
        /// Returns the Camera
        /// </summary>
        /// <returns></returns>
        public CameraController ReturnCamera()
        {
            return MainCamera;
        }

        /// <summary>
        /// Return the barrier list
        /// </summary>
        /// <returns></returns>
        public List<GameObject> ReturnBarrierList()
        {
            return Barriers;
        }




        /// <summary>
        /// Return the currently controled unit 
        /// </summary>
        /// <returns></returns>
        public Player ReturnCurrentPlayer()
        {
            return Players[CurrentPlayerIndex];
        }

        /// <summary>
        /// Check if the current Unit is AI or user controlled
        /// </summary>
        /// <returns></returns>
        public bool IsUser()
        {
            if (Players[CurrentPlayerIndex] is UserPlayer)
                return true;
            return false;
        }

        /// <summary>
        /// Change current Player's mode
        /// </summary>
        /// <param name="playerMode"></param>
        public void SetPlayerMode(string playerMode)
        {
            Players[CurrentPlayerIndex].SetMode(playerMode);
        }

        public int ReturnMapSize()
        {
            return MapSize;
        }


        public List<List<Tile>> ReturnMap()
        {
            return _map;

        }

        public List<Player> ReturnPlayers()
        {
            return GetComponent<PlayerGenerator>().ReturnPlayers();
        }

        /// <summary>
        /// ReturnCurrentState
        /// </summary>
        public BattleState ReturnState()
        {
            return _battlestate;
        }

        /// <summary>
        /// Return  Danger Level
        /// </summary>
        public DangerLevel ReturnDangerLevel()
        {
            return _dangerLevel;
        }


        /// <summary>
        /// SetCurrentState
        /// </summary>
        public void SetState(BattleState state)
        {
            _battlestate = state;
        }

        /// <summary>
        /// Return  Danger Level
        /// </summary>
        public void SetDangerLevel(DangerLevel dlevel)
        {
            _dangerLevel = dlevel;
        }

        #endregion
    }
}

