//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

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
