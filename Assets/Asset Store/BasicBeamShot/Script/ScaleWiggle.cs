//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class ScaleWiggle : MonoBehaviour {

        public float MaxWiggle = 1.0f;
        public float WiggleSpd = 0.5f;

        public Vector3 DefScale;
        private Vector3 TgtScale;
        private Vector3 PrevScale;
        private float TgtTime;

        // Use this for initialization
        private void Start () {
            DefScale = transform.localScale;
            TgtScale = DefScale;
            PrevScale = DefScale;
            TgtTime = 1.0f;
        }
	
        // Update is called once per frame
        private void Update () {
            if(TgtTime >= 1.0f)
            {
                TgtTime = 0.0f;
                float wig = Random.value*MaxWiggle;
                Vector3 wig3 = DefScale * wig;

                TgtScale = DefScale+-wig3*0.5f+wig3;
                PrevScale = transform.localScale;
            }else{
                TgtTime += WiggleSpd;
            }

            transform.localScale = Vector3.Lerp(PrevScale,TgtScale,TgtTime);
        }
    }
}
