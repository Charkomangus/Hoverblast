//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class ShotParticleEmitter : MonoBehaviour {

        public GameObject ShotParticle;
        public float ShotPower;
        public float Disturbance = 0;
        private float Rld;
        public float RldTime = 2.0f;
        public Color col = new Color(1,1,1);
        // Use this for initialization
        private void Start () {
            ShotPower = 0;
            Rld = 0;
        }
	
        // Update is called once per frame
        private void Update () {
            Rld -= 1.0f;
            if(Rld < 0.0f && ShotPower != 0)
            {		
                Rld = RldTime;
                float ShotPowerBuf = ShotPower;
                while(ShotPowerBuf >= 0)
                {	
                    //RandX
                    Quaternion q_rnd = Quaternion.AngleAxis((Random.value*Disturbance)-Disturbance*0.5f,this.transform.right);
				
                    //RandZ
                    q_rnd *= Quaternion.AngleAxis((Random.value*Disturbance)-Disturbance*0.5f,this.transform.up);

                    GameObject pat = (GameObject)GameObject.Instantiate(ShotParticle,Vector3.zero,q_rnd);
                    pat.GetComponent<LineRenderer>().SetColors(col, col);
                    pat.transform.parent = this.transform.parent;
                    ShotPowerBuf -= 1.0f;
                }
            }
        }
    }
}
