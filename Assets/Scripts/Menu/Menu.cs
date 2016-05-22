using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class Menu : MonoBehaviour
    {
        private Animator _animator;
        private CanvasGroup _canvasGroup;

        

        public void Awake()
        {
            
            _canvasGroup = GetComponent<CanvasGroup>();

            //Set a centre canvas position
            var rect = GetComponent<RectTransform>();
            rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
            _animator = GetComponent<Animator>();
        }

        void OnLevelWasLoaded(int level)
        {
            if(level == 1)
                if (!_animator.isInitialized)
                    _animator.Rebind();
        }

        /// <summary>
        /// Set up bool that determines which menu is open.
        /// </summary>
        public bool IsOpen
            {
            get { return _animator.GetBool("IsOpen"); }
            set { _animator.SetBool("IsOpen", value); }
        }


        /// <summary>
        /// Determine which menus will be interactable
        /// </summary>
        public void Update()
        {
            

            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
            else
                _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
          

        }
    }
}



