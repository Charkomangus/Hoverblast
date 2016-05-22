//                          >>>>>>><<<<>>>>>NOTICE<<<<<<<<<<<<<<<
//              This script is taken from the BasicBeamShot Unity assetpack.
//      While modified to serve the purpose of this project this scipt is not written by me.
//  Full licence and permission to do so can be found at https://unity3d.com/legal/copyright-policy

using UnityEngine;

namespace Assets.Asset_Store.BasicBeamShot.Script
{
    public class GeroBeam : MonoBehaviour {
        public GameObject HitEffect;
        private ShotParticleEmitter _shpEmitter;

        private float _nowLength;
        public float MaxLength = 16.0f;
        public float AddLength = 0.1f;
        public float Width = 10.0f;
        private LineRenderer _lr;
        private Vector3[] _fVec;
        private int _lrSize;
        private GeroBeamHit _hitObj;

        public float NowLengthGlobal;
        private BeamParam _bp;
        private GameObject _flash;
        private float _flashSize;
        // Use this for initialization
        private void Start () {
            _bp = GetComponent<BeamParam>();
            _lrSize = 16;
            _nowLength = 0.0f;
            _lr = GetComponent<LineRenderer>();
            _hitObj = transform.FindChild("GeroBeamHit").GetComponent<GeroBeamHit>();
            _shpEmitter = transform.FindChild("ShotParticle_Emitter").GetComponent<ShotParticleEmitter>();
            _flash = transform.FindChild("BeamFlash").gameObject;
            _fVec = new Vector3[_lrSize+1];
            _flashSize = _flash.transform.localScale.x;
            for (var i=0;i < _lrSize+1;i++)
            {
                _fVec[i] = transform.forward;
            }
        }
	
        // Update is called once per frame
        private void Update () {
            if (_bp.BEnd)
            {
                _bp.Scale *= 0.9f;
                _shpEmitter.ShotPower = 0.0f;
           
                Width *= 0.9f;
                if(Width < 0.01f)
                    Destroy(gameObject,2);
            }else{
                _shpEmitter.ShotPower = 1.0f;
            }

            _nowLength = Mathf.Min(1.0f,_nowLength+AddLength);
		
            Vector3 nowPos;

            _lr.SetWidth(Width*_bp.Scale,Width*_bp.Scale);
            _lr.SetColors(_bp.BeamColor, _bp.BeamColor);
            MaxLength = _bp.MaxLength;
            for (var i=_lrSize-1;i > 0;i--)
            {
                _fVec[i] = _fVec[i-1];
            }
            _fVec[0] = transform.forward;
            _fVec[_lrSize] = _fVec[_lrSize-1];
            var blockLen = MaxLength/_lrSize;

            for(var i=0;i < _lrSize;i++)
            {
                nowPos = transform.position;
                for(var j=0;j<i;j++)
                {
                    nowPos+=_fVec[j]*blockLen;
                }
                _lr.SetPosition(i,nowPos);
            }

            //Collision
            var bHitNow = false;
            NowLengthGlobal = _nowLength*_lrSize;

            if(Width >= 0.01f)
            {
                for(var i=0;i<_lrSize;i++)
                {
                    var workNlg = Mathf.Min(1.0f,NowLengthGlobal-i);

                    nowPos = transform.position;
                    for(var j=0;j<i;j++)
                    {
                        nowPos+=_fVec[j]*blockLen;
                    }


                    RaycastHit hit;
                    if(workNlg <= 0)
                        break;
                    var layerMask = ~(1 << LayerMask.NameToLayer("NoBeamHit") | 1 << 2);
                    if (!Physics.Raycast(nowPos, _fVec[i], out hit, blockLen*workNlg, layerMask)) continue;
                    _nowLength = ((blockLen*i)+hit.distance)/MaxLength;
                    _hitObj.transform.position = nowPos + _fVec[i] * hit.distance;
                    _hitObj.transform.rotation = Quaternion.AngleAxis(180.0f,transform.up)* transform.rotation;
                    bHitNow = true;
                    break;
                }
            }
            var shotFlashScale = _flashSize * Width * 5.0f;
            _flash.GetComponent<ScaleWiggle>().DefScale = new Vector3(shotFlashScale, shotFlashScale, shotFlashScale);
           
            gameObject.GetComponent<Renderer>().material.SetFloat("_AddTex",Time.frameCount*-0.05f*_bp.AnimationSpd*10);
            gameObject.GetComponent<Renderer>().material.SetFloat("_BeamLength",_nowLength);
            _flash.GetComponent<Renderer>().material.SetColor("_Color", _bp.BeamColor*2);
            _shpEmitter.col = _bp.BeamColor*2;
            _hitObj.col = _bp.BeamColor*2;
        }
    }
}
