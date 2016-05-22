//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class ShotParticle_Scale : MonoBehaviour {

        private LineRenderer LR;
        private float width;
        private float length;
        private float time;
        private Vector3 forwad;

        // Use this for initialization
        private void Start () {
            LR = transform.GetComponent<LineRenderer>();
            width = 1.0f;
            length = 0.0f;
            time = 0.0f;
            forwad = transform.forward;

            Quaternion ParentQua = transform.parent.rotation;
            Vector3 V = ParentQua * forwad;

            LR.SetPosition(0,transform.parent.position);
            LR.SetPosition(1,transform.parent.position+V*transform.parent.localScale.z * length);
            LR.SetWidth(transform.parent.localScale.x * width,transform.parent.localScale.x * width);
        }
	
        // Update is called once per frame
        private void Update () {
            Quaternion ParentQua = transform.parent.rotation;
            Vector3 V = ParentQua * forwad;
		
            LR.SetPosition(0,transform.parent.position);
            LR.SetPosition(1,transform.parent.position+V*transform.parent.localScale.z * length);
            LR.SetWidth(transform.parent.localScale.x * width,transform.parent.localScale.x * width);

            width = Mathf.Lerp(width,0,time*time);
            length += 0.075f*1.5f;

            time += 0.05f;
        }
    }
}
