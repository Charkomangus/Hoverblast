﻿//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class JumpScene : MonoBehaviour {

        public int scene_index = 0;
	
        // Use this for initialization
        private void Start () {
		
        }
	
        // Update is called once per frame
        private void Update () {
        }
	
        public void ChangeScene(){
            print("SceneChange:"+scene_index);
            Application.LoadLevel(scene_index);
        }
    }
}
