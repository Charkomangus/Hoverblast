using Assets.Scripts.MAIN_MANAGERS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu
{
    /// <summary>
    /// Handles which menu will be shown to the player.
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
       


        private StateManager _sm; //GamemanagerInstance
        public Animator MatchType;
        public Animator ExitMenu;
        public Animator LoadGame;
        public Menu CurrentMenu; //Public, so first default menu is set through the inspector
        public Menu tempMenu;
        


        //Initialise the Game Manager
        public void Awake()
        {
            _sm = StateManager.Instance;
            tempMenu = CurrentMenu;

        }

        //Call the Show Menu Method
        public void Start()
        {
            ShowMenu(CurrentMenu);
        }

       
        public void Update()
        {

            
                CurrentMenu.IsOpen = true;
                
                    
        }

     

        /// <summary>
        /// Check if the current menu is null. If not disable it. Then set the given menu as the current menu.
        /// </summary>
        /// <param name="menu"></param> Menu to be activated
        public void ShowMenu(Menu menu)
        {
            if (CurrentMenu != null)
                CurrentMenu.IsOpen = false;
            CurrentMenu = menu;
            CurrentMenu.IsOpen = true;
            OpenPanel("Reset");
        }

        /// <summary>
        /// If no menu is given assign the main menu as active
        /// </summary>
        public void ShowMenu()
        {
            if (CurrentMenu != null)
                CurrentMenu.IsOpen = false;
            CurrentMenu = tempMenu;
            CurrentMenu.IsOpen = true;
            OpenPanel("Reset");
        }


       

        /// <summary>
        /// Change the panels IsOpen bool.
        /// </summary>
        public void OpenPanel(string panel)
        {
            if (!ExitMenu.isInitialized)
                ExitMenu.Rebind();
            if (!MatchType.isInitialized)
                MatchType.Rebind();
            if (!LoadGame.isInitialized)
                LoadGame.Rebind();
            

            switch (panel)
            {
                case "MatchType":
                  
                    if (MatchType.isInitialized)
                        MatchType.SetBool("IsOpen", !MatchType.GetBool("IsOpen"));

                  
                    if(ExitMenu.isInitialized)
                        ExitMenu.SetBool("IsOpen", false);

                    if(LoadGame.isInitialized)
                        LoadGame.SetBool("IsOpen", false);
                    break;
                case "LoadGame":
                    if (MatchType.isInitialized)
                        MatchType.SetBool("IsOpen", false);
                    if (ExitMenu.isInitialized)
                        ExitMenu.SetBool("IsOpen", false);
                    if (LoadGame.isInitialized)
                        LoadGame.SetBool("IsOpen", !LoadGame.GetBool("IsOpen"));
                    
                    break;
                case "Exit":
                    if (MatchType.isInitialized)
                        MatchType.SetBool("IsOpen", false);
                    if (ExitMenu.isInitialized)
                        ExitMenu.SetBool("IsOpen", !ExitMenu.GetBool("IsOpen"));
                    if (LoadGame.isInitialized)
                        LoadGame.SetBool("IsOpen", false);
                   
                    break;
                case "Reset":
                    if(MatchType.isInitialized)
                        MatchType.SetBool("IsOpen", false);
                    if (ExitMenu.isInitialized)
                        ExitMenu.SetBool("IsOpen", false);
                    if (LoadGame.isInitialized)
                        LoadGame.SetBool("IsOpen", false);
                    break;
            }
        }

       
       
        

        /// <summary>
        /// Quit the application
        /// </summary>
        public void Quit()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
      
       


    }


}



