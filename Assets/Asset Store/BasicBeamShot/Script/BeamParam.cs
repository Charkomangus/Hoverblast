//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policyusing UnityEngine;

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class BeamParam : MonoBehaviour {
	
        public Color BeamColor = Color.white;
        public float AnimationSpd = 0.1f;
        public float Scale = 1.0f;
        public float MaxLength = 32.0f;
        public bool BEnd = false;
        public bool BGero = false;

        public void SetBeamParam(BeamParam param)
        {
            BeamColor = param.BeamColor;
            AnimationSpd = param.AnimationSpd;
            Scale = param.Scale;
            MaxLength = param.MaxLength;
        }

        private void Start () {
            var param = transform.root.gameObject.GetComponent<BeamParam>();

            if (param == null) return;
            BeamColor = param.BeamColor;
            AnimationSpd = param.AnimationSpd;
            Scale = param.Scale;
            MaxLength = param.MaxLength;
        }
    }
}
