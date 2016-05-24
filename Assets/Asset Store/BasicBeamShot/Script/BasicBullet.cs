//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class BasicBullet : MonoBehaviour {

        private Vector3 Vec = new Vector3(0,0,0.00005f);

        // Use this for initialization
        private void Start () {
            GetComponent<Rigidbody>().AddForce(Vec);
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag != "Bullet")
            {
                Destroy(this.gameObject);
            }
        }
        // Update is called once per frame
        private void Update () {


        }
    }
}
