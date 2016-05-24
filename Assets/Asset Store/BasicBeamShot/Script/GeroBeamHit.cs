//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class GeroBeamHit : MonoBehaviour {
        private GameObject ParticleA;
        private GameObject ParticleB;
        private GameObject HitFlash;
	
        private float PatA_rate;
        private float PatB_rate;

        private ParticleSystem PatA;
        private ParticleSystem PatB;
        public Color col;
       

        // Use this for initialization
        private void Start () {
            col = new Color(1, 1, 1);
            ParticleA = transform.FindChild("GeroParticleA").gameObject;
            ParticleB = transform.FindChild("GeroParticleB").gameObject;
            HitFlash = transform.FindChild("BeamFlash").gameObject;
            PatA = ParticleA.gameObject.GetComponent<ParticleSystem>();
#pragma warning disable 618
            PatA_rate = PatA.emissionRate;
#pragma warning restore 618
#pragma warning disable 618
            PatA.emissionRate = 0;
#pragma warning restore 618
            PatB = ParticleB.gameObject.GetComponent<ParticleSystem>();
#pragma warning disable 618
            PatB_rate = PatB.emissionRate;
#pragma warning restore 618
#pragma warning disable 618
            PatB.emissionRate = 0;
#pragma warning restore 618

            HitFlash.GetComponent<Renderer>().enabled = false;
        }
	
        // Update is called once per frame
        private void Update () {
            PatA.startColor = col;
            PatB.startColor = col;
            HitFlash.GetComponent<Renderer>().material.SetColor("_Color", col*1.5f);
        }
    }
}
