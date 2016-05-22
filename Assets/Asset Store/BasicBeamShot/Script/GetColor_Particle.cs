using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class GetColor_Particle : MonoBehaviour {

        // Use this for initialization
        private void Start () {
            ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
            ps.startColor = this.transform.root.gameObject.GetComponent<BeamParam>().BeamColor;
            ps.startSize *= this.transform.root.gameObject.GetComponent<BeamParam>().Scale;
        }
	
        // Update is called once per frame
        private void Update () {
	
        }
    }
}
