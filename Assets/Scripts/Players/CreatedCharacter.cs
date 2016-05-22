using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Players
{
    public class CreatedCharacter : MonoBehaviour
    {
        //Create character variables
        public InputField _nameInputField;
        public string _name;
        public string _appearance;
        public Weapon _weapon;
        public Armor _armor;
        public Armor _item;
        public Image _portrait;
        public Button _button;

        private void Awake()
        {
            _portrait = GetComponent<Image>();
            _button = GetComponent<Button>();
            _nameInputField = GetComponentInChildren<InputField>();
            _nameInputField.text = _nameInputField.placeholder.GetComponent<Text>().text;
        }

        

        // Update is called once per frame
        private void Update()
        {

            _name = _nameInputField.text;
        }
#region GET&SETS
        //Get Appearance
        public string GetAppearance()
        {
            return _appearance;
        }

        //Set Appearance
        public void SetAppearance(string appearance)
        {
            _appearance = appearance;
        }

        //Get Name
        public string GetName()
        {
            return _name;
        }

        //Set Name
        public void SetName(string nameGiven)
        {
            _name = nameGiven;
        }

        //Get Weapon
        public Weapon GetWeapon()
        {
            return _weapon;
        }

        //Set Weapon
        public void SetWeapon(Weapon weapon)
        {
            _weapon = weapon;
        }
        //Get Armor
        public Armor GetArmor()
        {
            return _armor;
        }

        //Set Armor
        public void SetArmor(Armor armor)
        {
            _armor = armor;
        }

        //Get Item
        public Armor GetItem()
        {
            return _item;
        }

        //Set Item
        public void SetItem(Armor item)
        {
            _item = item;
        }

        //Get Image
        public Image GetImage()
        {
            return _portrait;
        }

        //Set Image
        public void SetImage(Sprite portrait)
        {
            _portrait.sprite = portrait;
        }
        //GetInputField
        public string GetInput()
        {
            return _nameInputField.text;
        }
        //Set Input Field
        public void SetInput(string nameInputField)
        {
            _nameInputField.text = nameInputField;
        }
        //GetButton
        public Button GetButton()
        {
            return _button;
        }
        //Set Button
        public void SetButton(Button button)
        {
            _button = button;
        }
    }
#endregion
}
