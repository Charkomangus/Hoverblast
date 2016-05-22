using Assets.Scripts.MAIN_MANAGERS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class PauseManager : MonoBehaviour
    {


        public GameObject PauseMenu;       
        public GameObject OptionsMenu;
        private bool _isPaused;
        private bool _infostatus;
        private bool _infostatusplayer;
        private bool _infostatustile;

        // Use this for initialization
        public void Start ()
        {
            _isPaused = false;
            PauseMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            _infostatus = GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.GetBool("IsOpen");
            _infostatusplayer = GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.GetBool("IsOpenTile");
            _infostatustile = GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.GetBool("IsOpenPlayer");
        }
	
        // Update is called once per frame
        private void Update()
        {


            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (!OptionsMenu.activeSelf && !OptionsMenu.activeSelf)

                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        PauseMenu.SetActive(true);
                        //Close UI elements
                        _infostatus =GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.GetBool("IsOpen");
                        _infostatusplayer =GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.GetBool("IsOpenTile");
                        _infostatustile =GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.GetBool("IsOpenPlayer");
                        GameManager.Instance.GameUi.GetComponent<UiManager>().CloseInfoBox();
                        GameManager.Instance.GameUi.GetComponent<UiManager>().ActionAnimator.SetBool("IsOpen", false);
                        
                    }
                }
            }
            else
            {
                PauseMenu.SetActive(false);
                //Bring UI elements back to their original form
                GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.SetBool("IsOpen", _infostatus);
                GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.SetBool("IsOpenPlayer", _infostatustile);
                GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.SetBool("IsOpenTile", _infostatusplayer);
                GameManager.Instance.GameUi.GetComponent<UiManager>().ActionAnimator.SetBool("IsOpen", true);
            }

            //If either of the pause panels are active set IsPaused to true
            if (PauseMenu.activeSelf || OptionsMenu.activeSelf)
                _isPaused = true;
            else
            {
                _isPaused = false;
            }
        }

        //Return Pause status
        public bool IsPaused()
        {
            return _isPaused;
        }


        public void ResumeGame()
        {
            PauseMenu.SetActive(false);
            //Bring UI elements back to their original form
            GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.SetBool("IsOpen", _infostatus);
            GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.SetBool("IsOpenPlayer", _infostatustile);
            GameManager.Instance.GameUi.GetComponent<UiManager>().InfoBoxAnimator.SetBool("IsOpenTile", _infostatusplayer);
            GameManager.Instance.GameUi.GetComponent<UiManager>().ActionAnimator.SetBool("IsOpen", true);
        }
    }
}
