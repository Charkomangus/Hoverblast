using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class BillBoard : MonoBehaviour
    {
        private Camera LookAtCam;
	
        public float RandomRotate = 0;
        private float RndRotate;
        private Transform this_t_;

        private void Awake() {
            this_t_ = this.transform;
            RndRotate = Random.value*RandomRotate;
        }

        private void Update() {
            LookAtCam = Camera.main;
            if ( LookAtCam == null ) return;
            Transform cam_t = LookAtCam.transform;
		
            Vector3 vec = cam_t.position - this_t_.position;
            vec.x = vec.z = 0.0f;
            this_t_.LookAt(cam_t.position - vec); 
            this_t_.Rotate(Vector3.forward,RndRotate);
        }
    }
}