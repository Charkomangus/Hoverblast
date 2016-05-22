using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class OptionsMenu : Menu.Menu
    {

       
        /// <summary>
        /// Panels
        /// </summary>
        public List<GameObject> Panels;


       
        /// <summary>
        /// General Panel Variables
        /// </summary>


        /// <summary>
        /// Video Panel Variables
        /// </summary>
        private Vector2 _currentResolution;
        private string _currentGraphics;
        private readonly List<Vector2> _resolutions = new List<Vector2>();
        private readonly List<string> _graphicMode = new List<string>();
        public Text ResolutionText;
        public Text GraphicsText;
        //Setting up bools
        private bool _vSyncOn;
        private bool _fullscreenOn;
        private bool _hasApplied;
        //For enabling and disabling
        public GameObject ResolutionNext;
        public GameObject ResolutionPrevious;
        public GameObject GraphicsNext;
        public GameObject GraphicsPrevious;
        public Button Apply;

        /// <summary>
        /// Audio Panel Variables
        /// </summary>
        public Slider MusicSlider;
        public Slider AudioSlider;
        public Toggle MusicToggle;
        public Toggle AudToggle;
        private float _musicVolume;
        private float _audioVolume;
        public AudioSource MusicSource;
        public AudioSource SoundSource;

        //Setting up bools
        private bool _musicOn;
        private bool _audioOn;


        // Use this for initialization
        private void Start()
        {
          
            ShowPanel(Panels[1]);
            //Add standard resolutions
            _resolutions.Add(new Vector2(1600, 900));
            _resolutions.Add(new Vector2(1920, 1080));
            _resolutions.Add(new Vector2(2560, 1440));
            //Add standard resolutions
            _graphicMode.Add("Low");
            _graphicMode.Add("Medium");
            _graphicMode.Add("High");
            //Initialise resolution & Graphics to min
            _currentResolution = _resolutions[1];
            _currentGraphics = _graphicMode[1];
            //Initialise Settings
            _vSyncOn = true;
            _fullscreenOn = true;
            _hasApplied = true;
            _musicOn = true;
            _audioOn = true;

        }

        //Update is called once per frame
        private new void Update()
        {
            DisplayResolution();
            DisplayGraphics();

            //Check if settings have already been applied
            Apply.interactable = !_hasApplied;

            //Check where the sliders are
            MusicToggle.isOn = MusicSlider.value > 0;
            AudToggle.isOn = AudioSlider.value > 0;
            

        }

        /// <summary>
        /// Check which is the current resolution displayed and show it
        /// </summary>
        private void DisplayResolution()
        {
            ResolutionNext.SetActive(_currentResolution != _resolutions[_resolutions.Count - 1]);
            ResolutionPrevious.SetActive(_currentResolution != _resolutions[0]);
            ResolutionText.text = _currentResolution.x + " x " + _currentResolution.y;
        }

        /// <summary>
        /// Set first panel to be general
        /// </summary>
        public void SetPanel()
        {
            foreach (var panel in Panels)
            {
                panel.SetActive(false);
            }
            ShowPanel(Panels[1]);
        }


        /// <summary>
        /// Set sliders to Proper Levels
        /// </summary>
        public void SetSliders()
        {
            if (MusicSource != null)
                MusicSlider.value = MusicSource.volume;
           
            if (SoundSource != null)
                AudioSlider.value = SoundSource.volume;
        }

        /// <summary>
        /// Check which is the current graphics mode displayed and show it
        /// </summary>
        private void DisplayGraphics()
        {
            GraphicsNext.SetActive(_currentGraphics != "High");
            GraphicsPrevious.SetActive(_currentGraphics != "Low");
            GraphicsText.text = _currentGraphics;
        }


        /// <summary>
        /// Detect the current resolution and set it to be  whatever is int i away from it
        /// </summary>
        /// <param name="i"></param>
        public void ChangeResolution(int i)
        {
            var changed = false;
            for (var j = 0; j < _resolutions.Count; j++)
            {
                if (_currentResolution != _resolutions[j] || changed) continue;
                _currentResolution = _resolutions[j + i];
                changed = true;
                _hasApplied = false;
            }
        }

        /// <summary>
        /// Detect the current Graphics mode and set it to be the next or previous one
        /// </summary>
        /// <param name="i"></param>
        public void ChangeGraphics(int i)
        {
            if (i > 0)
                switch (_currentGraphics)
                {
                    case "Medium":
                        _currentGraphics = _graphicMode[_graphicMode.Count - 1];
                        break;
                    case "Low":
                        _currentGraphics = _graphicMode[1];
                        break;
                }
            else
                switch (_currentGraphics)
                {
                    case "High":
                        _currentGraphics = _graphicMode[_graphicMode.Count - 2];
                        break;
                    case "Medium":
                        _currentGraphics = _graphicMode[0];
                        break;
                }
            _hasApplied = false;
        }

        /// <summary>
        /// Change fullscreen setttings
        /// </summary>
        public void ToggleFullscreen()
        {
            _fullscreenOn = !_fullscreenOn;
            _hasApplied = false;
        }

        /// <summary>
        /// Change Vsync setttings
        /// </summary>
        public void ToggleVsync()
        {
            _vSyncOn = !_vSyncOn;
            _hasApplied = false;
        }

        /// <summary>
        /// Apply all the settings to the game
        /// </summary>
        public void ApplySettings()
        {
            Screen.SetResolution((int)_currentResolution.x, (int)_currentResolution.y, _fullscreenOn); // Set the resolution 
            QualitySettings.vSyncCount = _vSyncOn ? 1 : 0; // Check if vsync should be enabled or not and apply it
            switch (_currentGraphics)
            {
                case "High":
                    QualitySettings.SetQualityLevel(2, true);
                    break;
                case "Medium":
                    QualitySettings.SetQualityLevel(1, true);
                    break;
                default:
                    QualitySettings.SetQualityLevel(0, false);
                    break;
            }

            _hasApplied = true;
        }


        /// <summary>
        /// Check if the current panel is null. If not disable it. Then set the given panel as the current panel.
        /// </summary>
        /// <param name="Panel"></param>
        /// Menu to be activated
        public void ShowPanel(GameObject Panel)
        {
            foreach (var panel in Panels)
            {
                panel.SetActive(false);
            }
            foreach (var panel in Panels.Where(panel => panel == Panel))
            {
                panel.SetActive(true);
                break;
            }
        }

        /// <summary>
        /// Change the bool and update the slider
        /// </summary>
        public void ToggleMusic()
        {
            _musicOn = !_musicOn;
            MusicSlider.value = !_musicOn ? 0 : 1;
        }


        /// <summary>
        /// Change the bool and update the slider
        /// </summary>
        public void ToggleAudio()
        {
            _audioOn = !_audioOn;
            AudioSlider.value = !_audioOn ? 0 : 1;
        }
    }
}
