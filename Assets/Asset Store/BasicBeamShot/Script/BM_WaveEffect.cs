//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class BM_WaveEffect : MonoBehaviour {

        public float InSize = 0.0f;
        public float OutSize = 1.0f;
        public float Height = 0.0f;

        // Use this for initialization
        private void Start () {

        }
	
        // Update is called once per frame
        private void Update () {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i+=2) {
                float r;
                r = ((float)(i)/(float)vertices.Length)*4*Mathf.PI;
                vertices[i].x = Mathf.Cos(r)*(Mathf.Lerp(0,OutSize,InSize));
                vertices[i].y = 0;
                vertices[i].z = Mathf.Sin(r)*(Mathf.Lerp(0, OutSize, InSize));
                vertices[i+1].x = Mathf.Cos(r)*(OutSize);
                vertices[i+1].y = Height;
                vertices[i+1].z = Mathf.Sin(r)*(OutSize);
            }
            mesh.vertices = vertices;
        }
    }
}
