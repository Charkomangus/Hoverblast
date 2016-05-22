using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.MapCreator;
using Assets.Scripts.MAIN_MANAGERS;
using Assets.Scripts.Tiles;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Players
{
    public class Player : MonoBehaviour
    {

        public Vector2 GridPosition = Vector2.zero;
        protected Vector3 MoveDestination;

        //Player Variables
        public string PlayerName = "Unit";
        public float MoveSpeed = 0.1f;
        public int TeamNumber;
        public int ActionPoints = 2;
        public int Hp;
        private const int MaxHp = 100;
        public int BarrierHealth;
        public bool BarrierEnabled;
        private int _tempHp;
        private int _abilityCooldown;
        private ShootingManager _shootingManager;
        private Tile _currentTile;


        //States of Player
        public string Mode;
        public bool Alive;
        private bool _appliedArmorBuff;

        //movement animation
        public List<Vector3> PositionQueue = new List<Vector3>();
        protected Renderer RendererInstance;       

        //Player Equipment
        protected Armor Armor;
        protected Armor Accesory;
        public List<Weapon> Weapons = new List<Weapon>();
        protected GameObject Barrier;

        //Audio
        private AudioSource _source;
        //Children
        private Light _mainLight;
        private Canvas _ui;
        public GameObject Body;
        private Animator _animator;
        private ShootingManager _shootManager;
        private Healthbar healthbar;

        /// <summary>
        /// Set Player Stats using equipment and base numbers
        /// </summary>
        #region AttackChance
        private const float BaseAttackChance = 0.85f;

        protected float AttackChance
        {
            get
            {
                var attackChance = BaseAttackChance;

                if (Armor != null) attackChance += Armor.AlterAttackChance;
                if (Accesory != null) attackChance += Accesory.AlterAttackChance;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    attackChance += weapon.AlterAttackChance;
                    if (weapon.Type == WeaponSlotType.TwoHanded) break;
                }

                return attackChance;
            }
            set { }
        }
        #endregion
        #region Evade

        private const float BaseEvade = 0.05f;

        protected float Evade
        {
            get
            {
                var evade = BaseEvade;

                if (Armor != null) evade += Armor.AlterEvade;
                if (Accesory != null) evade += Accesory.AlterEvade;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    evade += weapon.AlterEvade;
                    if (weapon.Type == WeaponSlotType.TwoHanded) break;
                }
                if (evade < 0)
                    evade = 0;
                return evade;
            }
            set { }
        }
        #endregion
        #region Damage Reduction

        private const int BaseDamageReduction = 0;

        protected int DamageReduction
        {
            get
            {
                var damageReduction = BaseDamageReduction;

                if (Armor != null) damageReduction += Armor.AlterDamageReduction;
                if (Accesory != null) damageReduction += Accesory.AlterDamageReduction;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    damageReduction += weapon.AlterDamageReduction;
                    if (weapon.Type == WeaponSlotType.TwoHanded) break;
                }

                return damageReduction;
            }
            set { }
        }
        #endregion
        #region Damage
        private const int BaseDamage = 10;

        protected int Damage
        {
            get
            {
                var damage = BaseDamage;

                if (Armor != null) damage += Armor.AlterDamageBase;
                if (Accesory != null) damage += Accesory.AlterDamageBase;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    damage += weapon.AlterDamageBase;
                    if (weapon.WeaponName != "Shock Taser" && weapon.WeaponName != "Plasma Spear") continue;
                    if (Accesory != null) damage += Accesory.AlterMeleeDamage;
                    if (Armor != null) damage += Armor.AlterMeleeDamage;
                    damage += weapon.AlterMeleeDamage;
                }

                return damage;
            }
            set { }
        }
        #endregion
        #region DamageRoll
        private const int BaseDamageRoll = 6;

        protected int DamageRoll
        {
            get
            {
                var damageRoll = BaseDamageRoll;

                if (Armor != null) damageRoll += Armor.AlterDamageRollSides;
                if (Accesory != null) damageRoll += Accesory.AlterDamageRollSides;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    damageRoll += weapon.AlterDamageRollSides;
                    if (weapon.Type == WeaponSlotType.TwoHanded) break;
                }

                return damageRoll;
            }
            set { }
        }
        #endregion
        #region AttackActionCost
        private const int BaseAttackActionCost = 1;

        protected int AttackActionCost
        {
            get
            {
                var attackActionCost = BaseAttackActionCost;

                if (Armor != null) attackActionCost += Armor.AlterActionCost;
                if (Accesory != null) attackActionCost += Accesory.AlterActionCost;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    attackActionCost += weapon.AlterActionCost;
                    if (weapon.Type == WeaponSlotType.TwoHanded) break;
                }

                return attackActionCost;
            }
            set { }
        }
        #endregion
        #region Movement Per Action Point
        private const int BaseMovementPerActionPoint = 5;

        protected int MovementPerActionPoint
        {
            get
            {
                var movementPerActionPoint = BaseMovementPerActionPoint;

                if (Armor != null) movementPerActionPoint += Armor.AlterMovementPerActionPoint;
                if (Accesory != null) movementPerActionPoint += Accesory.AlterMovementPerActionPoint;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    movementPerActionPoint += weapon.AlterMovementPerActionPoint;
                    if (weapon.Type == WeaponSlotType.TwoHanded) break;
                }

                return movementPerActionPoint;
            }
            set { }
        }
        #endregion
        #region Attack Range
        private const int BaseAttackRange = 1;

        protected int AttackRange
        {
            get
            {
                var attackRange = BaseAttackRange;

                if (Armor != null) attackRange += Armor.AlterAttackRange;
                if (Accesory != null) attackRange += Accesory.AlterAttackRange;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    attackRange += weapon.AlterAttackRange;
                    if (weapon.Type == WeaponSlotType.TwoHanded) break;
                }

                return attackRange;
            }
            set {}
        }
        #endregion
        #region Action Cost
        private const int BaseActionCost = 1;

        protected int ActionCost
        {
            get
            {
                var actionCost = BaseActionCost;

                if (Armor != null) actionCost += Armor.AlterActionCost;
                if (Accesory != null) actionCost += Accesory.AlterActionCost;


                foreach (var weapon in Weapons.Where(weapon => weapon != null))
                {
                    actionCost += weapon.AlterActionCost;
                    if (weapon.Type == WeaponSlotType.TwoHanded) break;
                }

                return actionCost;
            }
            set { }
        }
        #endregion











        private void Awake()
        {
            healthbar = GetComponentInChildren<Healthbar>();
            _animator = GetComponent<Animator>();
            _source = GetComponent<AudioSource>();
            _shootingManager = GetComponent<ShootingManager>();
            _mainLight = GetComponentInChildren<Light>();
            _ui = GetComponentInChildren<Canvas>();
            Alive = true;
            Hp = MaxHp;
            MoveSpeed = 4.5f;
            BarrierHealth = 20;
        }

        // Use this for initialization
        private void Start()
        {
            _tempHp = Hp;
            _abilityCooldown = 0;
            _currentTile = new Tile();
            Mode = "Idle";
        }

        // Update is called once per frame
        public virtual void Update()
        {
            _currentTile = BattleManager.Instance.ReturnMap()[(int)GridPosition.x][(int)GridPosition.y];

            if (Hp <= 0)
                Alive = false;
            else if (Hp > MaxHp)
                Hp = MaxHp;

            if (!Alive)
                Dead();
            else
            {             
                CheckBarrier();

                //Light control
                if (this == BattleManager.Instance.ReturnCurrentPlayer() && Alive)
                {
                    if (_mainLight.intensity < 10)
                        _mainLight.intensity += 10*Time.deltaTime;

                   
                }
                else
                {
                    if (_mainLight.intensity > 0)
                    {
                        _mainLight.intensity -= 10*Time.deltaTime;
                    }
                    
                }


                Rotation(0, null);
                MedicArmorEffects();


                //Making sure the unit is centered at it's grid position
                if (PositionQueue.Count <= 0)
                {
                    GridPosition.x = (int) GridPosition.x;
                    GridPosition.y = (int) GridPosition.y;
                    transform.position = Vector3.Slerp(transform.position,
                        new Vector3((int) (GridPosition.x - BattleManager.Instance.ReturnMapSize()/2f), 1.2f,
                            (int) (BattleManager.Instance.ReturnMapSize()/2f - GridPosition.y)), 4*Time.deltaTime);
                }

                //ANimation of being hit
                if (_tempHp > Hp)
                    _animator.SetTrigger("IsHit");
                _tempHp = Hp;

            }
        }

        public virtual void TurnUpdate()
        {
            if (ActionPoints > 0) return;
            GameManager.Instance.GameUi.GetComponent<UiManager>().CloseInfoBox();
            if (Alive)
            {
                ActionPoints = 2;
                Cooldown();
                Mode = "Idle";
                _appliedArmorBuff = false;
                BattleManager.Instance.NextTurn();
            }
            else
            {
                ActionPoints = 0;
                Mode = "Idle";
                _appliedArmorBuff = true;
                BattleManager.Instance.NextTurn();
            }
        }


        private void CheckBarrier()
        {
            //Check to see if there is a barrier
            foreach (var child in transform.Cast<Transform>().Where(child => child.tag == "Barrier"))
            {
                Barrier = child.gameObject;
            }
            if (Barrier == null) return;
            Barrier.GetComponent<Animator>().SetBool("IsOpen", BarrierHealth > 0);
            
            BarrierEnabled = Barrier.GetComponent<Animator>().GetBool("IsOpen");
        }




        //Decrease the cooldown copunter each turn and make sure that it doesnt go past 0
        private void Cooldown()
        {
            if (BarrierEnabled) return;
            if (_abilityCooldown < 0)
                _abilityCooldown = 0;
            if (_abilityCooldown > 0)
                _abilityCooldown--;
        }

        /// <summary>
        /// Apply any effects that the armor would cause
        /// </summary>
        private void MedicArmorEffects()
        {
            if (Armor.ArmorName != "Medic Suit" || !Alive || _appliedArmorBuff) return;
            if (Hp + 5 > MaxHp)
                Hp = MaxHp;
            else
            {
                Hp += 5;
            }
            _appliedArmorBuff = true;
        }


        /// <summary>
        /// Rotate Unit to face their path or enemy
        /// </summary>
        public void Rotation(int type, Tile destTile)
        {
            switch (type)
            {
                case 0:
                    if (PositionQueue.Count >= 1)
                    {
                        var pathRotation = Quaternion.LookRotation(PositionQueue[0] - transform.position);
                        // Smoothly rotate towards the target point.
                        transform.rotation = Quaternion.Slerp(transform.rotation, pathRotation, 8*Time.deltaTime);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }
                    break;
                case 1:
                        var targetRotation = Quaternion.LookRotation(destTile.transform.position - transform.position);
                        // Snap rotate towards the target point.
                        transform.rotation = targetRotation;
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    break;
                default:
                        targetRotation = Quaternion.LookRotation(destTile.transform.position - transform.position);
                         // Snap rotate towards the target point.
                        transform.rotation = targetRotation;
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    break;
            }
        }

        /// <summary>
        /// Set unit into dead mode
        /// </summary>
        public void Dead()
        {
            _animator.SetBool("IsAlive", false);
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, 0.45f, transform.position.z), 1 * Time.deltaTime);
                  
            _mainLight.intensity = 0;            
            _ui.transform.gameObject.SetActive(false);
        }

       //Detectes when the mouse is hovered over the player and does stuff is the mouse is clicked
        void OnMouseOver()
        {
            if (BattleManager.Instance.ReturnCurrentPlayer().ReturnTeam() != ReturnTeam() && BattleManager.Instance.ReturnCurrentPlayer().IsUser())
            {                
                healthbar.ChangePercentageStatus(true, (int)((BattleManager.Instance.ReturnCurrentPlayer().ReturnAttackChance() - ReturnEvade()) * 100) + "% to Hit!");
            }
           
            if (Input.GetMouseButtonDown(0) && _currentTile.ReturnRenderType().transform.GetComponent<Renderer>().materials[0].color != Color.white)
            {
                Debug.Log("Colour is:" + _currentTile.ReturnRenderType().transform.GetComponent<Renderer>().materials[0].color);
                _currentTile.UseTile(_currentTile);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                GameManager.Instance.GameUi.GetComponent<UiManager>().OpenInfoBox(this);
            }
            else if (Input.GetMouseButtonDown(2))
            {
                Mode = "Idle";
                BattleManager.Instance.RemoveTileHighlights();
                GameManager.Instance.GameUi.GetComponent<UiManager>().CloseInfoBox();

            }
        }


        
      
                
             
                
        void OnMouseExit()
        {
            if(BattleManager.Instance.ReturnCurrentPlayer().IsUser())
                healthbar.ChangePercentageStatus(false, (int)((BattleManager.Instance.ReturnCurrentPlayer().ReturnAttackChance() - ReturnEvade()) * 100) + "% to Hit!");
        }

        #region Sets & Returns

        //Return The shooting manager
        public ShootingManager ReturnShootingManager()
        {
            return _shootManager;
        }

        //Return The cooldown
        public int ReturnBarrierHealth()
        {
            return BarrierHealth;
        }

        //Set The cooldown
        public void SetBarrierHealth(int health)
        {
            BarrierHealth = health;
        }

        //Set The cooldown
        public bool IsUser()
        {
            return this is UserPlayer;
        }

        //Return Barrier
        public bool ReturnBarrierStatus()
        {
            return BarrierEnabled;
        }

        //Return The players Name
        public string ReturnName()
        {
            return PlayerName;
        }

        //Set The players Name
        public void SetName(string playerName)
        {
            PlayerName = playerName;
        }
        //Return The cooldown
        public int ReturnCooldown()
        {
            return _abilityCooldown;
        }

        //Set The cooldown
        public void SetCooldown(int cooldown)
        {
            _abilityCooldown = cooldown;
        }

        //Set the Player Team
        public void SetTeam(int teamNumber)
        {
            TeamNumber = teamNumber;
        }

        //Gives a new destination to the player to move towards
        public void SetDestination(Vector3 newDestination)
        {
            MoveDestination = newDestination;
        }

        //Set the players Status
        public void SetMode(string newMode)
        {
            Mode = newMode;
        }

        //Return the current player status
        public string ReturnMode()
        {
            return Mode;
        }

        //Return the current Players movement
        public int ReturnMovement()
        {
            return MovementPerActionPoint;
        }

        //Return the current player Team
        public int ReturnTeam()
        {
            return TeamNumber;
        }

        //Set the Player Speed
        public void SetSpeed(float newSpeed)
        {
            MoveSpeed = newSpeed;
        }

        //Return the position queue
        public List<Vector3> ReturnPositionQueue()
        {
            return PositionQueue;
        }

        //Return the current player Attack Range
        public int ReturnAttackRange()
        {
            return AttackRange;
        }

        //Set the current player Damage Roll
        public void SetAttackRange(int attackRange)
        {
            AttackRange = attackRange;
        }

        
        //Return the current player grid position
        public Vector2 ReturnPosition()
        {
            return GridPosition;
        }

        //Set the current player grid position
        public void SetPosition(Vector2 position)
        {
            GridPosition = position;

        }

        //Set the current player Health
        public void SetHp(int hp)
        {
            Hp = hp;
        }

        //Return the current player Health
        public int ReturnHp()
        {
            return Hp;
        }

        //Return the max player Health
        public int ReturnMaxHp()
        {
            return MaxHp;
        }

        //Set the current player AP
        public void SetActionPoints(int actionPoints)
        {
            ActionPoints = actionPoints;
        }

        //Return the current player AP
        public int ReturnActionPoints()
        {
            return ActionPoints;
        }

        //Set the current player DamageBase
        public void SetDamage(int damage)
        {
            Damage = damage;
        }

        //Return the current player DamageBase
        public int ReturnDamage()
        {
            return Damage;
        }

        //Set the current player Attack Chance
        public void SetAttackChance(float attackChance)
        {
            AttackChance = attackChance;
        }

        //Return the current player Attack Chance
        public float ReturnAttackChance()
        {
            return AttackChance;
        }

        //Return the current player Damage Reduction
        public int ReturnDamageReduction()
        {
            return DamageReduction;
        }

        //Set the current player Damage Roll
        public void SetAttackDice(int damageRoll)
        {
            DamageRoll = damageRoll;
        }

        //Return the current player Damage Roll
        public float ReturnDamageRoll()
        {
            return DamageRoll;
        }

        //Return the current player Evade Chance
        public float ReturnEvade()
        {
            return Evade;
        }


        //Return the current player Action Cost
        public int ReturnActionCost()
        {
            return ActionCost;
        }

        //Player Equipment
        public void EquipPlayer(Armor armor, Armor accesory, Weapon weapon)
        {
            Armor = armor;
            Accesory = accesory;
            Weapons.Add(weapon);
        }

        //Return the Players Armor
        public Armor ReturnArmor()
        {
            return Armor; 
        }
        //Return the Players Accesory
        public Armor ReturnAccesory()
        {
            return Accesory;
        }
        //Return the Players Weapon
        public Weapon ReturnWeapon()
        {
            return Weapons[0];
        }

        //Return true if current player is alive
        public bool IsAlive()
        {
            return Alive;
        }

        //Set live status
        public void SetAlive(bool alive)
        {
            Alive = alive;
        }

        //Play Sound Effect
        public void PlaySoundEffect(AudioClip clip)
        {
            _source.PlayOneShot(clip);
        }


        //Get unit's rotation
        public Quaternion ReturnRotation()
        {
            return transform.rotation;
        }
        //Set unit's rotation
        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        //Get unit's current tile
        public Tile ReturnCurrentTile()
        {
            return _currentTile;
        }


        //Get unit's current tile
        public Healthbar ReturnHealthBar()
        {
            return healthbar;
        }



        #endregion
    }
}
