//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class MultiShooter : MonoBehaviour
    {

        public GameObject Shot1;
        public GameObject Shot2;
        public GameObject Wave;
        public float Disturbance = 0;

        public int ShotType = 0;
        private GameObject _bullet;
        private GameObject _nowShot;

        private void Start()
        {
            _nowShot = null;
        }


        private void Update()
        {
           
        }

        public void Shoot(int type)
        {
            switch (type)
            {
                case 0:
                    ShootType1();
                    break;
                case 1:
                    ShootType2();
                    break;
              
            }
        }

        //create BasicBeamShot
        private void ShootType1()
        {
            _bullet = Shot1;
            //Fire
            var s1 = (GameObject) Instantiate(_bullet, transform.position, transform.rotation);
            s1.GetComponent<BeamParam>().SetBeamParam(GetComponent<BeamParam>());

            var wav = (GameObject) Instantiate(Wave, transform.position, transform.rotation);
            wav.transform.localScale *= 0.25f;
            wav.transform.Rotate(Vector3.left, 90.0f);
            wav.GetComponent<BeamWave>().col = GetComponent<BeamParam>().BeamColor;
        }



        //create GeroBeam
        private void ShootType2()
        {
            var wav = (GameObject) Instantiate(Wave, transform.position, transform.rotation);
            wav.transform.Rotate(Vector3.left, 90.0f);
            wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;
            _bullet = Shot2;
            //Fire
            _nowShot = (GameObject) Instantiate(_bullet, transform.position, transform.rotation);
            EndBeam();
        }
        

        //End Beam
        private void EndBeam()
        {

            if (_nowShot != null)
            {
                _nowShot.GetComponent<BeamParam>().BEnd = true;
            }
        }
    }
}