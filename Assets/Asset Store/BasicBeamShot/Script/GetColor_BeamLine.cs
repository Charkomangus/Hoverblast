//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class GetColor_BeamLine : MonoBehaviour {

        // Use this for initialization
        private void Start () {
            BeamParam Parent = this.transform.root.gameObject.GetComponent<BeamParam>();
            if(Parent == null) return;
            BeamLine BL = this.gameObject.GetComponent<BeamLine>();
            BL.BeamColor = Parent.BeamColor;
		
            BL.StartSize = Parent.Scale*0.5f;
            BL.AnimationSpd = Parent.AnimationSpd;
            BL.MaxLength = Parent.MaxLength;

        }
	
        // Update is called once per frame
        private void Update () {
	
        }
    }
}
