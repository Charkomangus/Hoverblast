using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class AutoDestroy : MonoBehaviour {

        public float DestroyTime = 2.0f;

        // Use this for initialization
        private void Start () {
            Destroy(gameObject, DestroyTime);
        }
	
        // Update is called once per frame
        private void Update () {
	
        }
    }
}
