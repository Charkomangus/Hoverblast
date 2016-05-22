using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MAIN_MANAGERS
{
    public class IntroManager : MonoBehaviour
    {
        private StateManager _sm; //GamemanagerInstance

        //Initialise the Game Manager
        public void Awake()
        {
            _sm = StateManager.Instance;
            _sm.OnStateChange += HandleOnStateChange;
            Debug.Log("Current Game state: " + _sm.GameState);
            Cursor.visible = false;
        }

        // Use this for initialization
        private void Start()
        {
            _sm.SetGameState(GameState.Menu);
            Debug.Log("Current Game state: " + _sm.GameState);
        }

        // Update is called once per frame
        private void Update()
        {
           if(Input.GetKeyDown(KeyCode.Escape))
                LoadGame();
        }

        ///
        public void HandleOnStateChange()
        {
            Debug.Log("Handling state change to: " + _sm.GameState);
            Invoke("LoadGame", 10f);
        }

        /// <summary>
        /// Brings up the main menu
        /// </summary>
        public void LoadGame()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
