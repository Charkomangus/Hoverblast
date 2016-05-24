using System.Collections.Generic;
using Assets.Scripts.MAIN_MANAGERS;
using Assets.Scripts.Players;
using Assets.Scripts.SaveLoad;
using Assets.Scripts.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UiManager : MonoBehaviour
    {
        //Creating an Instance so scripts can access it's variables.
        public static UiManager Instance;


        // Name * Team
        public Text UnitName;
        public Text Team;


        //Action Points
        public Text ActionPoints;
        public Button FindPlayerButton;
        public Animator ActionAnimator;
        //Weapon
        public Text Weapon;
        public Text WeaponDamage;
        public Text WeaponRange;
        public Text ActionPointCost;

        //Armour
        public Text Armor;
        public Text ArmorDamageReduction;
        public Text Evade;
        public Text Movement;

        //Accesory
        public Text Accesory;

        //

        public Animator HitBox;

        //AbilityButtons
        public Button AttackButton;
        public Button MoveButton;
        public Button AbilityButton;
        public Text AbilityButtonText;
        public Button SkipTurnButton;

        //StatePanels
        public GameObject WinPanel;
        public GameObject LossPanel;
        public GameObject Resolution;

        //Resolution
        public Text TitleText;
        public List<Text> StarsResolution;
        public Text ScoreResolution;
        //Victory
        public List<Text> StarsVictory;
        public Text ScoreVictory;

        //OptionsMenu
        private PauseManager _pauseMenu;
        public GameObject UnitInformation;

        //Player Team Turn
        public Animator PlayerPvpTurnAnimator;
        public Animator PlayerTurnAnimator;
        private Player _tempPlayer;


        //Right Click Information Box
        public Animator InfoBoxAnimator;
        public Text InfoName;
        public Text InfoDescription;
        public Text InfoWeapon;
        public Text InfoWeaponDamage;
        public Text InfoWeaponRange;
        public Text InfoActionPointCost;
        public Text InfoArmor;
        public Text InfoArmorDamageReduction;
        public Text InfoEvade;
        public Text InfoMovement;
        public Text InfoAccesory;
        public Text InforPercentageHit;
        public Text InfoTeam;
        private float _percentageToHit;
        //Image
        public Image InfoImage;
        public Sprite AllyImage;
        public Sprite EnemyImage;
        public Sprite TileImage;
        public GameObject FadeIn;
        private Animator _animator;
        public Animator ChooseLevelAnimator;

        private void Awake()
        {
            Instance = this;
            _pauseMenu = GetComponentInChildren<PauseManager>();
            _animator = GetComponent<Animator>();
            UnitInformation.SetActive(false);
            FindPlayerButton.onClick.AddListener(FindPlayer);
            _tempPlayer = new AiPlayer();
            _tempPlayer.SetTeam(1);
        }

        //Show team turn animation of the teams change
        private void ShowPlayerTeamTurn(Player player)
        {
            if (_tempPlayer.ReturnTeam() != player.ReturnTeam())
            {
                PlayerTurnAnimator.SetTrigger("IsOpen");
                PlayerTurnAnimator.GetComponentInChildren<Text>().text = "Player " + (player.ReturnTeam() + 1) +
                                                                         " Turn!";
                _tempPlayer = player;
            }

        }


        //Set camera target to the current player
        private void FindPlayer()
        {
            BattleManager.Instance.ReturnCamera().SetTarget(BattleManager.Instance.ReturnCurrentPlayer().transform);
        }

        // Update is called once per frame
        private void Update()
        {
            if (SceneManager.GetActiveScene().name != "Game") return;
            switch (BattleManager.Instance.ReturnState())
            {
                case BattleState.Battle:
                    LossPanel.SetActive(false);
                    WinPanel.SetActive(false);
                    Resolution.SetActive(false);
                    UpdatePanel();
                    ButtonKeyboardInput();
                    UpdateButtons();
                    UnitInformation.SetActive(!_pauseMenu.IsPaused());
                    TurnOffUiForAi();
                    ShowPlayerTeamTurn(BattleManager.Instance.ReturnCurrentPlayer());
                    if (Input.GetKeyDown(KeyCode.C))
                        FindPlayer();
                    break;
                case BattleState.Win:
                    LossPanel.SetActive(false);
                    WinPanel.SetActive(true);
                    Resolution.SetActive(false);
                    VictoryPanel();
                    UnitInformation.SetActive(false);
                    break;
                case BattleState.Loss:
                    LossPanel.SetActive(true);
                    WinPanel.SetActive(false);
                    Resolution.SetActive(false);
                    UnitInformation.SetActive(false);
                    break;
                case BattleState.ResolutionPlayerOne:
                    LossPanel.SetActive(false);
                    WinPanel.SetActive(false);
                    Resolution.SetActive(true);
                    UnitInformation.SetActive(false);
                    ResolutionPanel(1);

                    break;
                case BattleState.ResolutionPlayerTwo:
                    LossPanel.SetActive(false);
                    WinPanel.SetActive(false);
                    Resolution.SetActive(true);
                    UnitInformation.SetActive(false);
                    ResolutionPanel(0);

                    break;
            }
        }


        /// <summary>
        /// Change buttons status depending on player
        /// </summary>
        private void UpdateButtons()
        {
            //Sets the attack button to be uninteractable if the player does not have enough action points to meet the requirement of their weapon
            AttackButton.interactable = BattleManager.Instance.ReturnCurrentPlayer().ReturnActionPoints() >=
                                        BattleManager.Instance.ReturnCurrentPlayer().ReturnActionCost();
            //Sets the move button to be uninteractable if the player is moving
            if (BattleManager.Instance.ReturnCurrentPlayer().ReturnPositionQueue().Count > 0)
            {
                AttackButton.interactable = false;
                SkipTurnButton.interactable = false;
                MoveButton.interactable = false;
            }
            else
            {
                SkipTurnButton.interactable = true;
                MoveButton.interactable = true;
            }
            //Sets the ability button to be off if there is a cooldown
            if (BattleManager.Instance.ReturnCurrentPlayer().ReturnCooldown() == 0 &&
                BattleManager.Instance.ReturnCurrentPlayer().ReturnPositionQueue().Count <= 0)
            {
                AbilityButton.interactable = true;
                AbilityButtonText.text = "";
            }
            else
            {
                AbilityButton.interactable = false;
                AbilityButtonText.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnCooldown() == 0
                    ? ""
                    : BattleManager.Instance.ReturnCurrentPlayer().ReturnCooldown().ToString();
            }
        }


        /// <summary>
        /// Press Buttons
        /// </summary>
        private void ButtonKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (AttackButton.interactable && BattleManager.Instance.ReturnCurrentPlayer().IsUser())
                    GameManager.Instance.GetComponent<Actions>().Attack();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (MoveButton.interactable && BattleManager.Instance.ReturnCurrentPlayer().IsUser())
                    GameManager.Instance.GetComponent<Actions>().Move();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (AbilityButton.interactable && BattleManager.Instance.ReturnCurrentPlayer().IsUser())
                    GameManager.Instance.GetComponent<Actions>().SpecialAbility();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (SkipTurnButton.interactable && BattleManager.Instance.ReturnCurrentPlayer().IsUser())
                    GameManager.Instance.GetComponent<Actions>().SkipTurn();
            }
        }






        /// <summary>
        /// Open the hit box
        /// </summary>
        /// <param name="text"></param>
        public void OpenHitBox(string text)
        {
            if (HitBox == null) return;
            if (HitBox.gameObject.transform.GetComponentInChildren<Text>() == null)
                HitBox.gameObject.AddComponent<Text>();

            HitBox.gameObject.transform.GetComponentInChildren<Text>().text = text;
            HitBox.SetTrigger("Hit");

        }

        /// <summary>
        /// ChooseLevelAnimator
        /// </summary>
        public void ShowChooseLevel(string status)
        {
            switch (status)
            {
                case "":
                    ChooseLevelAnimator.SetBool("IsOpen", !ChooseLevelAnimator.GetBool("IsOpen"));
                    break;
                case "True":
                    ChooseLevelAnimator.SetBool("IsOpen", true);
                    break;
                case "False":
                    ChooseLevelAnimator.SetBool("IsOpen", false);
                    break;
            }
        }

        /// <summary>
        /// ChooseLevelAnimator
        /// </summary>
        public void ShowChoosePvpLevel(string status)
        {
            switch (status)
            {
                case "":
                    PlayerPvpTurnAnimator.SetBool("IsOpen", !PlayerPvpTurnAnimator.GetBool("IsOpen"));
                    break;
                case "True":
                    PlayerPvpTurnAnimator.SetBool("IsOpen", true);
                    break;
                case "False":
                    PlayerPvpTurnAnimator.SetBool("IsOpen", false);
                    break;
            }
        }


        /// <summary>
        /// Open the hit box
        /// </summary>
        public void TurnOffUiForAi()
        {
            GetComponent<Animator>().SetBool("IsUser", BattleManager.Instance.ReturnCurrentPlayer().IsUser());
            ActionAnimator.SetBool("IsOpen", !GetComponentInChildren<PauseManager>().IsPaused() && BattleManager.Instance.ReturnCurrentPlayer().IsUser());
            if (!BattleManager.Instance.ReturnCurrentPlayer().IsUser())
            {
                for (int i = 0; i < BattleManager.Instance.ReturnPlayers().Count; i++)
                {
                    BattleManager.Instance.ReturnPlayers()[i].ReturnHealthBar().ChangePercentageStatus(false, "");
                }
            }

        }


        /// <summary>
        /// Open the correctt Panel and Update the information on the panel
        /// </summary>
        private void UpdatePanel()
        {

            UnitName.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnName();
            Team.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnTeam() == 0 ? " Team: A" : " Team: B";
            //Action Points
            ActionPoints.text = "Action Points Left: " +
                                BattleManager.Instance.ReturnCurrentPlayer().ReturnActionPoints();
            //Weapon
            Weapon.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnWeapon().WeaponName;
            WeaponDamage.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnDamage() + " - " +
                                (BattleManager.Instance.ReturnCurrentPlayer().ReturnDamage() +
                                 BattleManager.Instance.ReturnCurrentPlayer().ReturnDamageRoll());
            WeaponRange.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnAttackRange().ToString();
            ActionPointCost.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnActionCost().ToString();

            //Armour
            Armor.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnArmor().ArmorName;
            ArmorDamageReduction.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnDamageReduction().ToString();
            Evade.text = (int) (BattleManager.Instance.ReturnCurrentPlayer().ReturnEvade()*100) + "%";
            Movement.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnMovement().ToString();
            //Accesory
            Accesory.text = BattleManager.Instance.ReturnCurrentPlayer().ReturnAccesory().ArmorName;

        }

        /// <summary>
        /// Open the Info box and Update the information on in
        /// </summary>
        public void OpenInfoBox(Component target)
        {
            if (target is Player)
            {
                InfoBoxAnimator.SetBool("IsOpen", true);
                InfoBoxAnimator.SetBool("IsOpenTile", false);
                InfoBoxAnimator.SetBool("IsOpenPlayer", true);
                //Base info
                InfoName.text = target.GetComponent<Player>().ReturnName();
                InfoTeam.text = target.GetComponent<Player>().ReturnTeam() == 0 ? " Team: A" : " Team: B";
                if (target.GetComponent<Player>().ReturnTeam() !=
                    BattleManager.Instance.ReturnCurrentPlayer().ReturnTeam())
                {
                    InfoDescription.text = "An enemy unit. Destroy at all costs.";
                    InfoImage.sprite = EnemyImage;
                }
                else
                {
                    InfoDescription.text = "An allied unit. Teamwork is key.";
                    InfoImage.sprite = AllyImage;
                }

                //Weapon
                InfoWeapon.text = target.GetComponent<Player>().ReturnWeapon().WeaponName;
                InfoWeaponDamage.text = target.GetComponent<Player>().ReturnDamage() + " - " +
                                        (target.GetComponent<Player>().ReturnDamage() +
                                         target.GetComponent<Player>().ReturnDamageRoll());
                InfoWeaponRange.text = target.GetComponent<Player>().ReturnAttackRange().ToString();
                InfoActionPointCost.text = target.GetComponent<Player>().ReturnActionCost().ToString();

                //Armour
                InfoArmor.text = target.GetComponent<Player>().ReturnArmor().ArmorName;
                InfoArmorDamageReduction.text = target.GetComponent<Player>().ReturnDamageReduction().ToString();
                InfoEvade.text = (int) (target.GetComponent<Player>().ReturnEvade()*100) + "%";
                InfoMovement.text = target.GetComponent<Player>().ReturnMovement().ToString();
                //Accesory
                InfoAccesory.text = target.GetComponent<Player>().ReturnAccesory().ArmorName;
                //Percentage


                if (target.GetComponent<Player>().ReturnTeam() !=
                    BattleManager.Instance.ReturnCurrentPlayer().ReturnTeam())
                {
                    _percentageToHit = BattleManager.Instance.ReturnCurrentPlayer().ReturnAttackChance() -
                                       target.GetComponent<Player>().ReturnEvade();
                    InforPercentageHit.text = (int) (_percentageToHit*100) + "%";
                }
                else
                    InforPercentageHit.text = "N/A";
            }

            else if (target is Tile)
            {
                InfoBoxAnimator.SetBool("IsOpen", true);
                InfoBoxAnimator.SetBool("IsOpenTile", true);
                InfoBoxAnimator.SetBool("IsOpenPlayer", false);

                InfoImage.sprite = TileImage;

                switch (target.GetComponent<Tile>().ReturnType())
                {
                    case TileType.Normal:
                        InfoName.text = "Normal tile";
                        InfoDescription.text =
                            "A section of terrain with no obstacles whatsoever. Units can cross it easily.";
                        break;
                    case TileType.Difficult:
                        InfoDescription.text =
                            "A section of terrain that has some foliage that can interfere with the hover capability of your units. Unit's will struggle to cross this.";
                        InfoName.text = "Difficult tile";
                        break;
                    case TileType.VeryDifficult:
                        InfoDescription.text =
                            "A section of terrain that is full of foliage that can interfere with the hover capability of your units. Unit's will struggle significantly to cross this.";
                        InfoName.text = "Very difficult tile";
                        break;
                    case TileType.Impassible:
                        InfoDescription.text =
                            "A section of terrain full of rocks and obstacles. No hover unit may cross this.";
                        InfoName.text = "Impassible tile";
                        break;
                }
            }

            else
            {
                InfoBoxAnimator.SetBool("IsOpen", false);
                InfoBoxAnimator.SetBool("IsOpenTile", false);
                InfoBoxAnimator.SetBool("IsOpenPlayer", false);
            }
        }

        /// <summary>
        /// Close the info box
        /// </summary>
        public void CloseInfoBox()
        {
            InfoBoxAnimator.SetBool("IsOpen", false);
            InfoBoxAnimator.SetBool("IsOpenTile", false);
            InfoBoxAnimator.SetBool("IsOpenPlayer", false);
        }

        /// <summary>
        /// Activate the panels is open animation
        /// </summary>
        public void ShowPanel()
        {
            _animator.SetBool("IsOpen", !_animator.GetBool("IsOpen"));
        }


        /// <summary>
        /// Change the text and Star markers to indicate the players score
        /// </summary>
        public void VictoryPanel()
        {
            var score = 0;
            if (score == 0)
            {
                foreach (Text starText in StarsVictory)
                {
                    starText.transform.gameObject.SetActive(false);
                }
            }

            score = BattleManager.Instance.DetermineAliveInTeam(0);
            if (score > 4)
                ScoreVictory.text = "4/4";
            else
                ScoreVictory.text = score + "/4";
            for (var i = 0; i < StarsVictory.Count - score; i++)
            {
                StarsVictory[i].transform.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Change the text and Star markers to indicate the players score
        /// </summary>
        public void ResolutionPanel(int team)
        {
            var score = 0;
            if (score == 0)
            {
                for (var i = 0; i < StarsVictory.Count; i++)
                {
                    StarsResolution[i].transform.gameObject.SetActive(false);
                }
            }

            TitleText.text = "PLAYER " + (team + 1) + " WINS!";
            score = BattleManager.Instance.DetermineAliveInTeam(team);
            if (score > 4)
                ScoreResolution.text = "4/4";
            else
                ScoreResolution.text = score + "/4";
            for (int i = 0; i < StarsResolution.Count - score; i++)
            {
                StarsResolution[i].transform.gameObject.SetActive(true);
            }



        }

    }




}
       
