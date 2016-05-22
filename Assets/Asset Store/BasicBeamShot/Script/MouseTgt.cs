using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class MouseTgt : MonoBehaviour {

        // Use this for initialization
        private void Start () {
	
        }
	
        // Update is called once per frame
        private void Update () {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 dir= ray.direction.normalized;

            transform.LookAt(transform.position+dir*16.0f);
        }
    }
}
