using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.MapCreator;
using Assets.Scripts.Menu;
using Assets.Scripts.Players;
using Assets.Scripts.Sound;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MAIN_MANAGERS
{
    public class GameManager : MonoBehaviour
    {
        //Creating an Instance so scripts can access it's variables.
        public static GameManager Instance;


        private StateManager _sm;
        public GameObject GameUi;
        public GameObject MainMenuManager;
        private PauseManager _pauseManager;
        public AudioManager AudioManager;
        public Camera MenuCamera;

        //Importing all necessery variables
        public int MapSize;
        public int TeamSize;
        public bool Pvp;
        private int _level = 0;
        private bool Campaign;
        //Importing all necessery prefabs
        public GameObject Player1Tank;
        public GameObject Player1Jet;
        public GameObject Player2Tank;
        public GameObject Player2Jet;
        public GameObject AiTank;
        public GameObject AiJet;
       

        //List of player data
        public List<CreatedCharacter> CampaignList;
        public List<CreatedCharacter> PvPList;

      
        //SaveLoad variables
        private List<Player> _originalPlayers;
        private DangerLevel _dangerLevel;
        private BattleState _battleState;

        // Use this for initialization
        private void Awake()
        {
            Cursor.visible = true;
            Instance = this;
            _sm = StateManager.Instance;
            _sm.OnStateChange += HandleOnStateChange;
            DontDestroyOnLoad(gameObject);
            _pauseManager = GameUi.GetComponentInChildren<PauseManager>();
            if (FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            
                //Set default teams in case players do not wish to customise units
            for (var i = 0; i < CampaignList.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        CampaignList[i].SetWeapon(Weapon.FromKey(WeaponKey.BeamCannon));
                        CampaignList[i].SetAppearance("Tank");
                        CampaignList[i].SetArmor(Armor.FromKey(ArmorKey.MedicSuit));
                        CampaignList[i].SetItem(Armor.FromKey(ArmorKey.Scope));
                        break;
                    case 1:
                        CampaignList[i].SetWeapon(Weapon.FromKey(WeaponKey.LaserTurret));
                        CampaignList[i].SetAppearance("Jet");
                        CampaignList[i].SetArmor(Armor.FromKey(ArmorKey.MagnesiumPlating));
                        CampaignList[i].SetItem(Armor.FromKey(ArmorKey.ForceField));
                        break;
                    case 2:
                        CampaignList[i].SetWeapon(Weapon.FromKey(WeaponKey.SniperRifle));
                        CampaignList[i].SetAppearance("Jet");
                        CampaignList[i].SetArmor(Armor.FromKey(ArmorKey.StealthSuit));
                        CampaignList[i].SetItem(Armor.FromKey(ArmorKey.Medpack));
                        break;
                    case 3:
                        CampaignList[i].SetWeapon(Weapon.FromKey(WeaponKey.PlasmaSpear));
                        CampaignList[i].SetAppearance("Tank");
                        CampaignList[i].SetArmor(Armor.FromKey(ArmorKey.ExoSkeleton));
                        CampaignList[i].SetItem(Armor.FromKey(ArmorKey.Teleportation));
                        break;
                    default:
                        CampaignList[i].SetWeapon(Weapon.FromKey(WeaponKey.LaserTurret));
                        CampaignList[i].SetAppearance("Tank");
                        CampaignList[i].SetArmor(Armor.FromKey(ArmorKey.ExoSkeleton));
                        CampaignList[i].SetItem(Armor.FromKey(ArmorKey.Medpack));
                        break;
                }
            }
            for (var i = 0; i < PvPList.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.BeamCannon));
                        PvPList[i].SetAppearance("Tank");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.MedicSuit));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.Scope));
                        break;
                    case 1:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.LaserTurret));
                        PvPList[i].SetAppearance("Jet");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.MagnesiumPlating));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.ForceField));
                        break;
                    case 2:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.SniperRifle));
                        PvPList[i].SetAppearance("Jet");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.StealthSuit));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.Medpack));
                        break;
                    case 3:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.PlasmaSpear));
                        PvPList[i].SetAppearance("Tank");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.ExoSkeleton));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.Teleportation));
                        break;
                    case 4:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.LaserTurret));
                        PvPList[i].SetAppearance("Tank");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.ExoSkeleton));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.Teleportation));
                        break;
                    case 5:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.LaserTurret));
                        PvPList[i].SetAppearance("Jet");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.MagnesiumPlating));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.ForceField));
                        break;
                    case 6:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.SniperRifle));
                        PvPList[i].SetAppearance("Jet");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.MagnesiumPlating));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.Scope));
                        break;
                    case 7:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.Taser));
                        PvPList[i].SetAppearance("Tank");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.ExoSkeleton));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.Scope));
                        break;
                    default:
                        PvPList[i].SetWeapon(Weapon.FromKey(WeaponKey.LaserTurret));
                        PvPList[i].SetAppearance("Tank");
                        PvPList[i].SetArmor(Armor.FromKey(ArmorKey.ExoSkeleton));
                        PvPList[i].SetItem(Armor.FromKey(ArmorKey.Medpack));
                        break;
                }
                
            }
        }


        private void Update()
        {
            if (Instance == null)
                Instance = this;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
               
                    GameUi.SetActive(true);   
                    MenuCamera.gameObject.SetActive(false); 
                    GetComponent<Actions>().SetSpecial();            
                    MainMenuManager.SetActive(false);
                    SetGameAudio();              
                    break;
                case "Menu":                   
                    GameUi.SetActive(false);
                    MenuCamera.gameObject.SetActive(true);
                    MainMenuManager.SetActive(true);
                    
                    break;
                case "MapCreatorScene":
                    GameUi.SetActive(false);
                    MainMenuManager.SetActive(false);
                    break;
            }
        }


        private void OnLevelWasLoaded(int level)
        {
            switch (level)
            {
                case 1:
                    
                    break;
                case 2:
                        BattleManager.Instance.GeneratePlayers(Pvp, AiTank, AiJet, Player1Tank, Player1Jet, Player2Tank,
                            Player2Jet, Pvp ? PvPList : CampaignList, null, false);
                    
                    break;
            }
        }


        /// <summary>
        /// Change the Game State to the Game Scene
        /// </summary>
        public void StartGame(bool pvp)
        {
            
            Pvp = pvp;
            //Start game scene
            _sm.SetGameState(GameState.Game);
            Debug.Log(_sm.GameState);
            if(Campaign)
                _level = 0;
           
        }
        

        /// <summary>
        /// Set audio surces of players
        /// </summary>
        public void SetGameAudio()
        {
            var players = BattleManager.Instance.ReturnPlayers();
            foreach (var player in players)
            {
                if (player.GetComponent<AudioSource>() == null)
                    player.gameObject.AddComponent<AudioSource>();
                player.GetComponent<AudioSource>().volume = AudioManager.ReturnSoundVolume();
            }
            var barriers = BattleManager.Instance.ReturnBarrierList();
          
            foreach (var barrier in barriers.Where(barrier => barrier != null))
            {
                if (barrier.GetComponent<AudioSource>() == null)
                    barrier.AddComponent<AudioSource>();
                barrier.GetComponent<AudioSource>().volume = AudioManager.ReturnSoundVolume();
            }
        }

        /// <summary>
        /// Change the Game State to the Game Scene
        /// </summary>
        public void SaveGame( )
        {
            _dangerLevel = BattleManager.Instance.ReturnDangerLevel();
            _battleState = BattleManager.Instance.ReturnState();
            _originalPlayers = BattleManager.Instance.ReturnPlayers();         
            MapSaveLoad.Save(MapSaveLoad.CreateMapContainer(BattleManager.Instance.ReturnMap()), "SavedLevel.xml");
        }



        /// <summary>
        /// Change the Game State to the Game Scene
        /// </summary>
        public void NextLevel()
        {
            if (Campaign)
            {
                if (_level >= 2)
                    BackToMainMenu();
                else
                {
                    _level ++;

                    BattleManager.Instance.GeneratePlayers(Pvp, AiTank, AiJet, Player1Tank, Player1Jet, Player2Tank,
                        Player2Jet, Pvp ? PvPList : CampaignList, null, false);
                   //Start game scene
                    _sm.SetGameState(GameState.Game);
                    Debug.Log(_sm.GameState);
                }
            }
            else
            {
                if (Pvp)
                {
                    BattleManager.Instance.GeneratePlayers(Pvp, AiTank, AiJet, Player1Tank, Player1Jet, Player2Tank,
                        Player2Jet, Pvp ? PvPList : CampaignList, null, false);
                  
                    //Start game scene
                    _sm.SetGameState(GameState.Game);
                    Debug.Log(_sm.GameState);
                }
                else
                {
                    GameUi.GetComponent<UiManager>().ShowChoosePvpLevel("");
                }
            }
            
        }

        //Starts the game with no other consideration of bools and things
        public void SimpleStartLevel()
        {
            
           BattleManager.Instance.GeneratePlayers(Pvp, AiTank, AiJet, Player1Tank, Player1Jet, Player2Tank,Player2Jet, Pvp ? PvPList : CampaignList, null, false);
           
            //Start game scene
            _sm.SetGameState(GameState.Game);
                Debug.Log(_sm.GameState);
            
        }
    

        /// <summary>
        /// Change the Game State to the Game Scene
        /// </summary>
        public void RestartLevel()
        {
           
            BattleManager.Instance.GeneratePlayers(Pvp, AiTank, AiJet, Player1Tank, Player1Jet, Player2Tank, Player2Jet, Pvp ? PvPList : CampaignList, _originalPlayers, true);
           


            //Start game scene
            _sm.SetGameState(GameState.Game);
            Debug.Log(_sm.GameState);
        }


        public void BackToMainMenu()
        {
            SceneManager.LoadScene("Menu");
            MainMenuManager.GetComponent<MenuManager>().ShowMenu();
            _level = 0;
        }


        
        public void HandleOnStateChange()
        {
            Invoke("ChangeScene", 0f);
        }


        /// <summary>
        /// Brings up the main menu
        /// </summary>
        public void ChangeScene()
        {
            SceneManager.LoadScene("Game");
           
        }

        /// <summary>
        /// QuitGame
        /// </summary>
        public void QuitGame()
        {
           StateManager.Instance.OnApplicationQuit();
            Application.Quit();
        }

        /// <summary>
        /// SetAiFightMode
        /// </summary>
        public void IsCampaign(bool mode)
        {
            Campaign = mode;
        }

        /// <summary>
        /// Returns Level
        /// </summary>
        /// <returns></returns>
        public void SetLevel(int selectedLevel)
        {
            _level = selectedLevel;
        }

        /// <summary>
        /// Return level
        /// </summary>
        /// <returns></returns>
        public int ReturnLevel()
        {
            return _level;
        }

        /// <summary>
        /// Return PauseManager
        /// </summary>
        /// <returns></returns>
        public PauseManager ReturnPauseManager()
        {
            return _pauseManager;
        }

    }
}
