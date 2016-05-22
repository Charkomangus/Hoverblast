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
