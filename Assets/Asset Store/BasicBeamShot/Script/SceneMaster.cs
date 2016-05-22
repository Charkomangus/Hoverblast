using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class SceneMaster : MonoBehaviour {

        // Use this for initialization
        private void Start () {
	
        }
	
        // Update is called once per frame
        private void Update () {
	
        }

        private void OnControllerColliderHit(ControllerColliderHit hit){
            if(hit.gameObject.tag == "SceneChanger"){
                print("SceneChanger_Detected");
                JumpScene js = hit.transform.GetComponent<JumpScene>();
                if(js != null)
                {
                    js.ChangeScene();
                }
            }
        }

    }
}
