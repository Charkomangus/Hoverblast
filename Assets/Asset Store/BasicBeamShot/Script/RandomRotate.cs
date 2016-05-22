using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class RandomRotate : MonoBehaviour {

        private float rot;
        private float add_rot;
        // Use this for initialization
        private void Start () {
            rot = Random.value*360.0f;
            add_rot = Random.Range(360.0f*2,360.0f*10);
        }
	
        // Update is called once per frame
        private void Update () {

            transform.Rotate(0,0,rot);
            rot += add_rot;
        }
    }
}
