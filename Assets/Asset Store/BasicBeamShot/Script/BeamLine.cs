//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class BeamLine : MonoBehaviour {

        public float MaxLength = 1.0f;
        public float StartSize = 1.0f;
        public float AnimationSpd = 0.1f;
        public Color BeamColor = Color.white;

        private float NowAnm;
        private LineRenderer line;

        private float NowLength;

        private bool bStop;

        public float GetNowLength()
        {
            return NowLength;
        }
        public void StopLength(float length)
        {
            NowLength = length;
            bStop = true;
        }

        private void LineFunc()
        {
            if(!bStop)
                NowLength = Mathf.Lerp(0,MaxLength,NowAnm);
            float width = Mathf.Lerp(StartSize,0,NowAnm);
            line.SetWidth(width,width);
            float length = NowLength;
            line.SetPosition(0,transform.position);
            line.SetPosition(1,transform.position+(transform.forward*length));
        }

        // Use this for initialization
        private void Start () {
            line = GetComponent<LineRenderer>();
            line.SetColors(BeamColor,BeamColor);
            NowAnm = 0;
            NowLength = 0;
            bStop = false;
            LineFunc();
        }
	
        // Update is called once per frame
        private void Update () {

            NowAnm+=AnimationSpd;

            if(NowAnm > 1.0)
            {
                Destroy(this.gameObject);
            }
            LineFunc();
        }
    }
}
