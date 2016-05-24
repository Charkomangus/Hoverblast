//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy


using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class GetColor_Flash : MonoBehaviour {

        // Use this for initialization
        private void Start () {
            BeamParam bp = this.transform.root.gameObject.GetComponent<BeamParam>();
            SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
            sr.color = bp.BeamColor;
            Light li = this.gameObject.GetComponent<Light>();
            li.color = bp.BeamColor;
            li.range *= bp.Scale;
        }
	
        // Update is called once per frame
        private void Update () {
	
        }
    }
}
