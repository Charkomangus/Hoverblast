using System;
using Assets.Scripts.CameraInput;
using Assets.Scripts.MAIN_MANAGERS;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Players
{
    /// <summary>
    /// Governs the UI componments that surround individual units
    /// </summary>
    public class Healthbar : MonoBehaviour
    {
        private CameraController _sourceCamera;
        private Player _parent;
        private int _originalHealth;
        private CanvasGroup _canvasGroup;
        public GameObject HealthbarObject;
        public GameObject SelectedImage;
        public Text HealthText;
        private Vector3 _newScale;
        private Animator PercentageAnimator;
        private Animator HitAnimator;


       

        // Use this for initialization
        private void Start ()
        {
            PercentageAnimator = transform.FindChild("PercentagePanel").GetComponent<Animator>();
            HitAnimator = transform.FindChild("HitPanel").GetComponent<Animator>();
            _sourceCamera = BattleManager.Instance.ReturnCamera();
            _parent = transform.parent.GetComponent<Player>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _originalHealth = _parent.ReturnHp();
        }
	
        // Update is called once per frame
        private void Update () {
            SetActive();
            SetHealthBar();
            SetHealthText();
            if (_sourceCamera != null)
            {
                transform.rotation = _sourceCamera.transform.rotation;
            }

        }


        /// <summary>
        /// Adjust health bar to represent its parent pl;ayers current health
        /// </summary>
        private void SetActive()
        {
            if (_parent == BattleManager.Instance.ReturnCurrentPlayer())
            {
                _canvasGroup.alpha = 1;
                SelectedImage.SetActive(true);
                SelectedImage.transform.localScale = new Vector3(1.02f, 1.02f, 1.02f);
            }
            else
            {
                _canvasGroup.alpha = 0.7f;
                SelectedImage.SetActive(false);
            }
        }



        /// <summary>
        /// Adjust health bar to represent its parent pl;ayers current health
        /// </summary>
        private void SetHealthBar()
        {
            //Check if the player has more than 0 health. If it does show the health remaining otherwise set it to zero
            _newScale = _parent.ReturnHp() > 0 ? new Vector3(_parent.ReturnHp() * 0.01f, 1, 1) : new Vector3(0, 1, 1);
            HealthbarObject.transform.localScale =  Vector3.Lerp(HealthbarObject.transform.localScale, _newScale, 6 * Time.deltaTime) ;
        }

        /// <summary>
        /// Set Text to display the units health
        /// </summary>
        private void SetHealthText()
        {
            if (_parent.ReturnHp() > 0)
                HealthText.text = _parent.ReturnHp().ToString();
            else
                HealthText.text = "DEAD";
        }

        //Trigget the hit animation
        public void TriggetHitCounter(string text)
        {
            HitAnimator.GetComponentInChildren<Text>().text = text;
            HitAnimator.SetTrigger("Hit");
        }

        //Change the percentage panel status
        public void ChangePercentageStatus(bool status, string text)
        {
            if (PercentageAnimator == null) return;
            PercentageAnimator.GetComponentInChildren<Text>().text = text;
                PercentageAnimator.SetBool("IsOpen", status);
        }

       
    }
}
