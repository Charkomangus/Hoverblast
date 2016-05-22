using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Players
{
    public class CharacterCreator : MonoBehaviour
    {

        //Appearance
        public Image CharacterPortrait;
        public Sprite TankImage;
        public Sprite JetImage;
        public Sprite DefaultImage;
        public Text AppereanceText;

        
        //Explanation
        public Text ExplanationName;
        public Text ExplanationDescription;
        public Text ExplanationStats;

        //Name
        public InputField NameInputField;
        public Text TeamName;
        public InputField TeamNameInputField;

        //Equipment
        public Text WeaponName;
        public Text ArmorName;
        public Text ItemName;
        private int _equipedWeapon;
        private int _equipedArmor;
        private int _equipedItem;
       
        private List<Armor> _armourList;
        private List<Armor> _accesoryList;
        private List<Weapon> _weaponList;
        private Animator _animator;
        public Animator AnimatorCreate;

        
        

        //List of Characters
        public List<CreatedCharacter> Characters;
        private int _selectedCharacter = 0;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Open menu
        /// </summary>
        public void OpenCreator()
        {
            _animator.SetBool("IsOpen", !_animator.GetBool("IsOpen"));
            AnimatorCreate.SetBool("IsOpen", !_animator.GetBool("IsOpen"));
        }

        // Use this for initialization
        private void Start()
        {
            for (int i = 0; i < Characters.Count; i++)
            {       
                

                var i1 = i;
                Characters[i].GetButton().onClick.AddListener(() =>SetCharacter(i1, Characters[i1].GetName(), Characters[i1].GetAppearance(),
                                Characters[i1].GetWeapon(), Characters[i1].GetArmor(), Characters[i1].GetItem()));

            }

            _weaponList = new List<Weapon>
            {
                Weapon.FromKey(WeaponKey.LaserTurret),
                Weapon.FromKey(WeaponKey.BeamCannon),
                Weapon.FromKey(WeaponKey.SniperRifle),
                Weapon.FromKey(WeaponKey.SRiuS),
                Weapon.FromKey(WeaponKey.Taser),
                Weapon.FromKey(WeaponKey.PlasmaSpear)
            };
            _armourList = new List<Armor>
            {
                Armor.FromKey(ArmorKey.MagnesiumPlating),
                Armor.FromKey(ArmorKey.ExoSkeleton),
                Armor.FromKey(ArmorKey.MedicSuit),
                Armor.FromKey(ArmorKey.StealthSuit)
            };
            _accesoryList = new List<Armor>
            {
                Armor.FromKey(ArmorKey.Teleportation),
                Armor.FromKey(ArmorKey.Scope),
                Armor.FromKey(ArmorKey.Medpack),
                Armor.FromKey(ArmorKey.ForceField)
            };

        }

       

        // Update is called once per frame
        private void Update()
        {
            foreach (var character in Characters)
            {
                switch (character.GetAppearance())
                {
                    case "Tank":
                        character.SetImage(TankImage);
                        break;
                    case "Jet":
                        character.SetImage(JetImage);
                        break;
                    default:
                        
                        character.SetImage(DefaultImage);
                        break;
                }
            }
            TeamName.text = TeamNameInputField.text;
        }

        /// <summary>
        /// Apply the character creation
        /// </summary>
        public void ApplyChanges()
        {
            var selected = Characters[_selectedCharacter];
            selected.SetName(NameInputField.text);
            selected.SetInput(NameInputField.text);
           
            selected.SetWeapon(_weaponList[_equipedWeapon]);
            selected.SetArmor(_armourList[_equipedArmor]);
            selected.SetItem(_accesoryList[_equipedItem]);
            if (CharacterPortrait.sprite == TankImage)
                selected.SetAppearance("Tank");
            else if (CharacterPortrait.sprite == JetImage)
                selected.SetAppearance("Jet");
        }
        /// <summary>
        /// Set the Create a Character menu to open with the correct  parametres
        /// </summary>
        public void SetCharacter(int character, string characterName, string type, Weapon weapon, Armor armor, Armor item)
        {
            //Set name and appearance
            NameInputField.text = characterName;
            _selectedCharacter = character;
        
            if (type == "Tank")
            {
                CharacterPortrait.sprite = TankImage;
                AppereanceText.text = "HOVER TANK";
                ExplanationName.text = "Hover Tank";
                ExplanationDescription.text = "";
                ExplanationStats.text = "";
            }
            else
            {
                CharacterPortrait.sprite = JetImage;
                AppereanceText.text = "HOVER JET";
                ExplanationName.text = "Hover Jet";
                ExplanationDescription.text = "";
                ExplanationStats.text = "";
            }
            
            //Set the equiped weapon as the one given
            for (var i = 0; i < _weaponList.Count; i++)
                if (weapon.WeaponName == _weaponList[i].WeaponName)
                    _equipedWeapon = i;
            //Set the equiped armor as the one given
            for (var i = 0; i < _armourList.Count; i++)
                if (armor.ArmorName == _armourList[i].ArmorName)
                    _equipedArmor = i;
            //Set the equiped accesory as the one given
            for (var i = 0; i < _accesoryList.Count; i++)
                if (item.ArmorName == _accesoryList[i].ArmorName)
                    _equipedItem = i;
         
            
            
            WeaponName.text = weapon.WeaponName.ToUpper();
            ArmorName.text = armor.ArmorName.ToUpper();
            ItemName.text = item.ArmorName.ToUpper();
            
        }

        /// <summary>
        /// Detect the current Appearance and set it to the other one
        /// </summary>
        public void ChangeAppearance()
        {
            if (CharacterPortrait.sprite == TankImage)
            {
                CharacterPortrait.sprite = JetImage;
                AppereanceText.text = "HOVER JET";
                ExplanationName.text = "Hover Jet";
                ExplanationDescription.text = "";
                ExplanationStats.text = "";
            }
            else
            {
                CharacterPortrait.sprite = TankImage;
                AppereanceText.text = "HOVER TANK";
                ExplanationName.text = "Hover Tank";
                ExplanationDescription.text = "";
                ExplanationStats.text = "";
            }
        }

        /// <summary>
        /// Cycle through the Weapons
        /// </summary>
        public void ChangeWeapon(int i)
        {
            _equipedWeapon += i;
            if (_equipedWeapon < 0)
                _equipedWeapon = _weaponList.Count - 1;
            if (_equipedWeapon > _weaponList.Count - 1)
                _equipedWeapon = 0;

            ExplanationName.text = _weaponList[_equipedWeapon].WeaponName.ToUpper();
            ExplanationDescription.text = _weaponList[_equipedWeapon].WeaponExplanation;
            ExplanationStats.text = _weaponList[_equipedWeapon].WeaponStats;
            WeaponName.text = _weaponList[_equipedWeapon].WeaponName.ToUpper();
        }


        /// <summary>
        /// Cycle through the Armors
        /// </summary>
        public void ChangeArmor(int i)
        {
            _equipedArmor += i;
            if (_equipedArmor < 0)
                _equipedArmor = 3;
            if (_equipedArmor > 3)
                _equipedArmor = 0;

            ExplanationName.text = _armourList[_equipedArmor].ArmorName.ToUpper();
            ExplanationDescription.text = _armourList[_equipedArmor].ArmorExplanation;
            ExplanationStats.text = _armourList[_equipedArmor].ArmorStats;
            ArmorName.text = _armourList[_equipedArmor].ArmorName.ToUpper();
        }

        /// <summary>
        /// Cycle through the Items
        /// </summary>
        public void ChangeItem(int i)
        {
            _equipedItem += i;
            if (_equipedItem < 0)
                _equipedItem = 3;
            if (_equipedItem > 3)
                _equipedItem = 0;

            ExplanationName.text = _accesoryList[_equipedItem].ArmorName.ToUpper();
            ExplanationDescription.text = _accesoryList[_equipedItem].ArmorExplanation;
            ExplanationStats.text = _accesoryList[_equipedItem].ArmorStats;
            ItemName.text = _accesoryList[_equipedItem].ArmorName.ToUpper();
        }


        

        /// <summary>
        /// Return Equiped Weapon
        /// </summary>
        public Weapon ReturnWeapon()
        {
            return _weaponList[_equipedWeapon];
        }

        /// <summary>
        /// Return Equiped Armor
        /// </summary>
        public Armor ReturnArmor()
        {
            return _armourList[_equipedArmor];
        }

        /// <summary>
        /// Return Equiped Armor
        /// </summary>
        public Armor ReturnItem()
        {
            return _accesoryList[_equipedItem];
        }

        /// <summary>
        /// Return Appearance
        /// </summary>
        public string ReturnAppearance()
        {
            return CharacterPortrait.sprite == TankImage ? "Tank" : "Jet";
        }

        /// <summary>
        /// Return Name
        /// </summary>
        public string ReturnName()
        {
            return NameInputField.text;
        }

    }
}


