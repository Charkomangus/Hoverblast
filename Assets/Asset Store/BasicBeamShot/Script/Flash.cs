﻿//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class Flash : MonoBehaviour {

        public float MaxSize = 1.0f;
        public float AnimationSpd = 0.1f;

        private float NowAnm = 0;
        private float t = 0;

        private Light pl;

        // Use this for initialization
        private void Start () {
            Vector3 m_scale = new Vector3(0,0,0);
            transform.localScale = m_scale;
            pl = (Light)this.gameObject.GetComponent<Light>();
        }
	
        // Update is called once per frame
        private void Update () {
            float s = Mathf.Lerp(0,MaxSize,Mathf.Min(t,1.0f));
            s = Mathf.Lerp(s,MaxSize/2,Mathf.Min(t-1.0f,1.0f));
            s = Mathf.Lerp(s,0.0f,NowAnm);
            t+=0.25f;

            Vector3 m_scale = new Vector3(s,s,s);
            transform.localScale = m_scale;

            if(pl != null)
            {
                pl.intensity = s*0.1f;
            }

            NowAnm += AnimationSpd;
            if(NowAnm > 1.0f)Destroy(this.gameObject);
        }
    }
}
