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
