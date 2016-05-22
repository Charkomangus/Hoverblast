using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class JumpScene : MonoBehaviour {

        public int scene_index = 0;
	
        // Use this for initialization
        private void Start () {
		
        }
	
        // Update is called once per frame
        private void Update () {
        }
	
        public void ChangeScene(){
            print("SceneChange:"+scene_index);
            Application.LoadLevel(scene_index);
        }
    }
}
