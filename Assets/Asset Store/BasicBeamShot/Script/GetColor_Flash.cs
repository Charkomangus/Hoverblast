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
